using DataAggregator.Processor.Configuration;
using DataAggregator.Processor.Services;
using DataAggregator.Processor.Services.DataStorage;
using DataAggregator.Processor.Services.Prediction;
using DataAggregator.Processor.Services.Processing.Factory;
using DataAggregator.Processor.Services.Registration;
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
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = "DataAggregator Processor API", Version = "v1" }));

// Register health checks
builder.Services.AddHealthChecks();

// Configure prediction service
builder.Services.Configure<PredictionServiceConfiguration>(builder.Configuration.GetSection("PredictionService"));

// Configure HTTP clients
builder.Services.AddHttpClient<IRegistrationServiceClient, RegistrationServiceClient>("RegistrationClient", client =>
{
    string registrationEndpoint = builder.Configuration["PredictionService:RegistrationServiceUrl"] ?? "http://localhost:5001";
    client.BaseAddress = new Uri(registrationEndpoint);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Register services
builder.Services.AddScoped<IDataRepository, InfluxV3Repository>();
builder.Services.AddSingleton<IDataProcessorFactory, DataProcessorFactory>();
builder.Services.AddScoped<IMachinePredictionProcessor, MachinePredictionProcessor>();

// Register background service
builder.Services.AddHostedService<PredictionBackgroundService>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DataAggregator Processor API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

try
{
    Log.Information("Starting the processor application...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Processor application failed to start.");
}
finally
{
    Log.CloseAndFlush();
}
