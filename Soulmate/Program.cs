using LoggerService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using Soulmate.Extensions;
using Soulmate.Helper.Seed;
using Soulmate.SignalR;
using SoulmateDAL.Data;
using SoulmateDAL.Entities;

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
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManger = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE [Connections]");
    await Seed.SeedUsers(userManager, roleManger);
}
catch (Exception ex)
{
    var logger = services.GetService<ILogger<Program>>();
    logger.LogError(ex, "Error occurred during migration");
}

app.Run();

