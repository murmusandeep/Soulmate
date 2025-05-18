using CloudinaryDotNet;
using LoggerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Models;
using Soulmate.Helper;
using Soulmate.SignalR;
using SoulmateBLL;
using SoulmateBLL.Interfaces;
using SoulmateDAL;
using SoulmateDAL.Data;
using SoulmateDAL.Interfaces;

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
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });

            services.AddScoped<IUsersBLL, UsersBLL>();
            services.AddScoped<IUsersDAL, UsersDAL>();
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddScoped<IAccountBLL, AccountBLL>();
            services.AddScoped<IAccountDAL, AccountDAL>();

            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

            services.AddSingleton(provider =>
            {
                var config = provider.GetRequiredService<IOptions<CloudinarySettings>>().Value;
                var account = new Account(config.CloudName, config.ApiKey, config.ApiSecret);
                return new Cloudinary(account);
            });

            services.AddScoped<IPhotoBLL, PhotoBLL>();
            services.AddScoped<IPhotoDAL, PhotoDAL>();

            services.AddScoped<LogUserActivity>();

            services.AddScoped<ILikesBLL, LikesBLL>();
            services.AddScoped<ILikesDAL, LikesDAL>();

            services.AddScoped<IMessageBLL, MessageBLL>();
            services.AddScoped<IMessageDAL, MessageDAL>();

            services.AddScoped<IAdminBLL, AdminBLL>();
            services.AddScoped<IAdminDAL, AdminDAL>();

            services.AddSignalR();
            services.AddSingleton<PresenceTracker>();

            return services;
        }
    }
}
