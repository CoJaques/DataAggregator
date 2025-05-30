using DataAggregator.Collector.DataCollector.Abstraction;
using DataAggregator.Collector.DataCollector.Connectors.CapnProto;
using DataAggregator.Collector.DataCollector.Connectors.OpenCN;
using DataAggregator.Collector.DataCollector.DataStorage;
using DataAggregator.Collector.DataCollector.DataStorage.Influx;
using DataAggregator.Collector.DataCollector.LocalStorage;
using DataAggregator.Collector.DataCollector.Registration;
using Microsoft.Extensions.Options;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure Serilog from appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = "DataAggregator Collector API", Version = "v1" }));

// Get collector type from configuration
string collectorType = builder.Configuration["CollectorType"] ?? "OpenCN";

// Setup configuration based on collector type
SetupConfiguration(builder);

// Configure HTTP clients
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("RegistrationClient", client =>
{
    string registrationEndpoint = builder.Configuration["RegistrationService:Endpoint"] ?? "http://localhost:5001";
    client.BaseAddress = new Uri(registrationEndpoint);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Register health checks
builder.Services.AddHealthChecks();

// Register common services
builder.Services.AddSingleton<DataBufferService>(sp =>
{
    int bufferSize = builder.Configuration.GetValue<int>("BufferSettings:MaxBufferSize", 10000);
    return new DataBufferService(bufferSize);
});

builder.Services.AddSingleton<RegistrationService>(sp =>
{
    IHttpClientFactory httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    HttpClient httpClient = httpClientFactory.CreateClient("RegistrationClient");

    string? registrationEndpoint = builder.Configuration["RegistrationService:Endpoint"];

    if (string.IsNullOrEmpty(registrationEndpoint))
    {
        Log.Warning("Registration service endpoint not configured, using default: http://localhost:5001/api/DeviceRegistration/register");
        registrationEndpoint = "http://localhost:5001/api/DeviceRegistration/register";
    }

    return new RegistrationService(httpClient, registrationEndpoint);
});

// Register collector-specific services based on CollectorType
if (collectorType.Equals("OpenCN", StringComparison.OrdinalIgnoreCase))
{
    // Setup the collector initialization service
    builder.Services.AddSingleton<CollectorInitializationService>(sp =>
    {
        RegistrationService registrationService = sp.GetRequiredService<RegistrationService>();
        OpenCnCollectorConfiguration config = sp.GetRequiredService<IOptions<OpenCnCollectorConfiguration>>().Value;
        return new CollectorInitializationService(registrationService, config);
    });

    // Setup data source connector
    builder.Services.AddSingleton<IDataSourceConnector>(sp =>
    {
        OpenCnCollectorConfiguration config = sp.GetRequiredService<IOptions<OpenCnCollectorConfiguration>>().Value;
        return new CapnProtoConnector(config);
    });

    // Setup data repository with initialization service
    builder.Services.AddSingleton<IDataRepository>(sp =>
    {
        CollectorInitializationService initService = sp.GetRequiredService<CollectorInitializationService>();
        return new InfluxDbRepository(initService);
    });

    // Setup OpenCN collector service
    builder.Services.AddSingleton<CollectorService>(sp =>
    {
        OpenCnCollectorConfiguration config = sp.GetRequiredService<IOptions<OpenCnCollectorConfiguration>>().Value;
        IDataSourceConnector dataSourceConnector = sp.GetRequiredService<IDataSourceConnector>();
        IDataRepository dataRepository = sp.GetRequiredService<IDataRepository>();
        CollectorInitializationService initService = sp.GetRequiredService<CollectorInitializationService>();
        DataBufferService dataBufferService = sp.GetRequiredService<DataBufferService>();

        return new CollectorService(
            dataSourceConnector,
            dataRepository,
            initService,
            dataBufferService,
            config);
    });
}
else
{
    Log.Fatal($"Unsupported collector type: {collectorType}");
    throw new InvalidOperationException($"Unsupported collector type: {collectorType}");
}

WebApplication app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DataAggregator Collector API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

// Start the collector service on application startup
app.Lifetime.ApplicationStarted.Register(async () =>
{
    try
    {
        Log.Information("Starting collector service...");

        // First initialize the collector service
        CollectorInitializationService initService = app.Services.GetRequiredService<CollectorInitializationService>();
        await initService.InitializeAsync();

        // Then start the collector service
        CollectorService collectorService = app.Services.GetRequiredService<CollectorService>();
        await collectorService.StartAsync();
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, "Failed to start collector service.");
    }
});

// Stop the collector service on application shutdown
app.Lifetime.ApplicationStopping.Register(async () =>
{
    try
    {
        Log.Information("Stopping collector service...");
        CollectorService collectorService = app.Services.GetRequiredService<CollectorService>();
        await collectorService.StopAsync();
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Error while stopping collector service.");
    }
});

try
{
    Log.Information("Starting the collector application...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Collector application failed to start.");
}
finally
{
    Log.CloseAndFlush();
}

static void SetupConfiguration(WebApplicationBuilder builder)
{
    string? collectorType = builder.Configuration["CollectorType"];

    if (collectorType?.Equals("OpenCN", StringComparison.OrdinalIgnoreCase) == true)
    {
        // Bind specifically to OpenCnCollectorConfiguration
        builder.Services.Configure<OpenCnCollectorConfiguration>(builder.Configuration.GetSection("Collector"));
    }
    else
    {
        Log.Warning("Collector type not specified or unsupported, application will close");
        throw new InvalidOperationException("Collector type not specified or unsupported.");
    }
}
