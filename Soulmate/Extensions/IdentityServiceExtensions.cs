using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared.Security;
using System.Text;

namespace Soulmate.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            var jwtSettings = config.GetSection("JwtSettings");

            var tokenKey = jwtSettings["TokenKey"];
            if (string.IsNullOrEmpty(tokenKey) || tokenKey.Length < 32)
            {
                throw new ArgumentException("Token key must be at least 32 characters long and not null.");
            }

            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = key,
                            ValidateIssuer = true,
                            ValidIssuer = issuer,
                            ValidateAudience = true,
                            ValidAudience = audience,
                            ValidateLifetime = true
                        };
                    });

            services.AddScoped<ITokenHelper, TokenHelper>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
