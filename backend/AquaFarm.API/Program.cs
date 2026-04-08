using AquaFarm.API;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("client", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });

// swagger - To test APIs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database initializer
builder.Services.AddScoped<DatabaseInitializer>();

// Other service registrations
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.ConfigureServices();

var app = builder.Build();

app.UseCors("client");

// Enable Swagger only in Development for testing
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Initialize the database at startup
using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    await dbInitializer.InitializeAsync();
}

app.Logger.Log(LogLevel.Information, "The application has started.");

// Use your custom error handling middleware
app.UseErrorHandling();

app.MapControllers();

app.UseStaticFiles();

// Health Probe route
app.MapGet("/healthz", () => "ok");

app.Run();
