using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoulmateDAL.Entities;
using SoulmateDAL.Interfaces;

namespace SoulmateDAL
{
    public class AccountDAL : IAccountDAL
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountDAL(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.NormalizedUserName == username.ToUpper());
        }

        public async Task<AppUser?> GetUser(string username)
        {
            return await _userManager.Users
                .Include(P => P.Photos)
                .SingleOrDefaultAsync(u => u.NormalizedUserName == username.ToUpper());
        }

        public async Task<IdentityResult> Register(AppUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<bool> CheckUserValid(AppUser appUser, string password)
        {
            return await _userManager.CheckPasswordAsync(appUser, password);
        }

        public async Task<IdentityResult> AddUserRole(AppUser appUser)
        {
            return await _userManager.AddToRoleAsync(appUser, "Member");
        }
    }
}
