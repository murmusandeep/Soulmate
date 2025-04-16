using LoggerService;
using NLog;
using Soulmate.Extensions;

var builder = WebApplication.CreateBuilder(args);

// NLog config
LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddControllers();

// Global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(); // Optional but helpful

var app = builder.Build();

// Exception handler
app.UseExceptionHandler(); // Triggers GlobalExceptionHandler

if (app.Environment.IsProduction())
    app.UseHsts();

// Custom request logging middleware
app.UseMiddleware<RequestLoggingMiddleware>();

// HTTPS & CORS
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Controller routing
app.MapControllers();

app.Run();

