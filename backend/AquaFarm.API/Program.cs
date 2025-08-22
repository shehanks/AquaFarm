using AquaFarm.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// swagger - To test APIs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database initializer
builder.Services.AddScoped<DatabaseInitializer>();

// Other service registrations
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.ConfigureServices();

var app = builder.Build();

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

// Health Probe route
app.MapGet("/healthz", () => "ok");

app.Run();
