using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.DataTransferObject;
using SoulmateDAL.Entities;

namespace Shared.Security
{
    public class TokenHelper : ITokenHelper
    {
        private readonly SymmetricSecurityKey _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly UserManager<AppUser> _userManager;

        public TokenHelper(IConfiguration config, UserManager<AppUser> userManager)
        {
            var jwtSettings = config.GetSection("JwtSettings");

            var tokenKey = jwtSettings["TokenKey"];
            _issuer = jwtSettings["Issuer"] ?? throw new ArgumentNullException("Issuer is missing from configuration.");
            _audience = jwtSettings["Audience"] ?? throw new ArgumentNullException("Audience is missing from configuration.");

            if (string.IsNullOrEmpty(tokenKey) || tokenKey.Length < 32)
            {
                throw new ArgumentException("Token key must be at least 32 characters long and not null.");
            }

            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            _userManager = userManager;
        }

        public async Task<string> CreateToken(MemberDto user)
        {
            if (user.Username == null)
                throw new Exception("No username for user");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Username),
                new Claim("UserId", user.Id.ToString())
            };

            var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (appUser == null)
                throw new Exception("User not found.");
            var roles = await _userManager.GetRolesAsync(appUser);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var cred = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = cred,
                Issuer = _issuer,
                Audience = _audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
