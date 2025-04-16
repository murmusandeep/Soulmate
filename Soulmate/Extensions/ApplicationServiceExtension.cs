using Microsoft.EntityFrameworkCore;
using SoulmateBLL.Interfaces;
using SoulmateBLL;
using SoulmateDAL;
using SoulmateDAL.Data;
using SoulmateDAL.Interfaces;
using LoggerService;
using Shared.Security;

namespace Soulmate.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("sqlConnection"), builder => builder.MigrationsAssembly("SoulmateDAL"));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                .WithOrigins("https://localhost:4200", "http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader());
            });

            services.AddScoped<IUsersBLL, UsersBLL>();
            services.AddScoped<IUsersDAL, UsersDAL>();
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddScoped<IAccountBLL, AccountBLL>();
            services.AddScoped<IAccountDAL, AccountDAL>();

            return services;
        }
    }
}
