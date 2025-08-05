using System.Text.Json;
using DataAggregator.Processor.Configuration;
using DataAggregator.Processor.Services;
using DataAggregator.Processor.Services.DataStorage;
using DataAggregator.Processor.Services.Prediction;
using DataAggregator.Processor.Services.Processing.Factory;
using DataAggregator.Processor.Services.Registration;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure Serilog from appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = "DataAggregator Processor API", Version = "v1" }));

if (!builder.Environment.IsDevelopment())
{
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
    });
}

// Register health checks
builder.Services.AddHealthChecks();

// Configuration management
string appSettingsJson = File.ReadAllText("appsettings.json");
using var doc = JsonDocument.Parse(appSettingsJson);
JsonElement predictionServiceElement = doc.RootElement.GetProperty("PredictionService");
string json = predictionServiceElement.GetRawText();

var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
options.Converters.Add(new ProcessorDescriptionJsonConverter());
PredictionServiceConfiguration? predictionConfig = JsonSerializer.Deserialize<PredictionServiceConfiguration>(json, options);
if (predictionConfig == null)
    throw new Exception("Failed to deserialize PredictionServiceConfiguration");
builder.Services.AddSingleton(predictionConfig);

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

if (!app.Environment.IsDevelopment())
{
    app.UseForwardedHeaders();
}

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
