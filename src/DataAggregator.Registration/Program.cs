using System.Net.Sockets;
using DataAggregator.Registration.DeviceManagement.Persistence;
using DataAggregator.Registration.DeviceManagement.Persistence.Repositories;
using DataAggregator.Registration.DeviceManagement.Services;
using DataAggregator.Registration.InfluxService.Configuration;
using DataAggregator.Registration.InfluxService.Services;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure Serilog from appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog(); // Use Serilog as the logging provider

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = "DataAggregator API", Version = "v1" }));

// Configure forwarded headers for reverse proxy scenarios
if (!builder.Environment.IsDevelopment())
{
    builder.Services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
    });
}

// Configure Entity Framework Core with PostgreSQL
string? pgHost = builder.Configuration["PGHOST"];
string? pgPort = builder.Configuration["PGPORT"];
string? pgDb = builder.Configuration["PGDATABASE"];
string? pgUser = builder.Configuration["PGUSER"];
string? pgPassword = builder.Configuration["PGPASSWORD"];

if (string.IsNullOrEmpty(pgHost) || string.IsNullOrEmpty(pgDb) || string.IsNullOrEmpty(pgUser) || string.IsNullOrEmpty(pgPassword))
{
    Log.Fatal("PostgreSQL environment variables are not properly configured.");
    throw new InvalidOperationException("PostgreSQL environment variables are not properly configured.");
}

string connectionString = $"Host={pgHost};Port={pgPort};Database={pgDb};Username={pgUser};Password={pgPassword}";

builder.Services.AddDbContext<RegistrationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Expose HealthCheck endpoint
builder.Services.AddHealthChecks()
    .AddDbContextCheck<RegistrationDbContext>();

// Bind InfluxEndpoints configuration
builder.Services.Configure<InfluxEndpointsConfiguration>(builder.Configuration.GetSection("Influx"));

// Register application services
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IDeviceRegistrationService, DeviceRegistrationService>();
builder.Services.AddScoped<IInfluxEndpointProviderService, InfluxEndpointProviderService>();

WebApplication app = builder.Build();

// Apply pending migrations at startup
using (IServiceScope scope = app.Services.CreateScope())
{
    RegistrationDbContext dbContext = scope.ServiceProvider.GetRequiredService<RegistrationDbContext>();
    try
    {
        Log.Information("Applying database migrations...");
        dbContext.Database.Migrate();
        Log.Information("Database is up to date.");
    }
    catch (Exception ex)
    {
        if (ex.InnerException is SocketException)
            Log.Fatal(ex, "Unable to connect to Database.");
        else
            Log.Fatal(ex, "An error occurred while applying database migrations.");

        throw;
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DataAggregator API v1");
        c.RoutePrefix = string.Empty;
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseForwardedHeaders(); // Use forwarded headers in production
}

app.MapControllers();
app.MapHealthChecks("/health");

try
{
    Log.Information("Starting the application...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start.");
}
finally
{
    Log.CloseAndFlush();
}
