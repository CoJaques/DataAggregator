using DataAggregator.Registration.Repositories;
using DataAggregator.Registration.Services;
using DataAggregator.Shared;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = "DataAggregator API", Version = "v1" }));

// Configure Entity Framework Core with PostgreSQL
string? pgHost = builder.Configuration["PGHOST"];
string? pgPort = builder.Configuration["PGPORT"];
string? pgDb = builder.Configuration["PGDATABASE"];
string? pgUser = builder.Configuration["PGUSER"];
string? pgPassword = builder.Configuration["PGPASSWORD"];

if (string.IsNullOrEmpty(pgHost) || string.IsNullOrEmpty(pgDb) || string.IsNullOrEmpty(pgUser) || string.IsNullOrEmpty(pgPassword))
{
    throw new InvalidOperationException("PostgreSQL environment variables are not properly configured.");
}

string connectionString = $"Host={pgHost};Port={pgPort};Database={pgDb};Username={pgUser};Password={pgPassword}";

builder.Services.AddDbContext<RegistrationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddDbContext<RegistrationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Expose HealthCheck endpoint
builder.Services.AddHealthChecks()
    .AddDbContextCheck<RegistrationDbContext>();

// Bind InfluxEndpoints configuration
builder.Services.Configure<List<InfluxEndpointConfiguration>>(builder.Configuration.GetSection("InfluxEndpoints"));

// Register application services
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IDeviceRegistrationService, DeviceRegistrationService>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DataAggregator API v1");
        c.RoutePrefix = string.Empty;
    });
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();
