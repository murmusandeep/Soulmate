using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoulmateDAL.Entities;
using SoulmateDAL.Interfaces;

namespace SoulmateDAL
{
    public class AdminDAL : IAdminDAL
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminDAL(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IList<string>> GetUserRoles(AppUser appUser)
        {
            return await _userManager.GetRolesAsync(appUser);
        }

        public async Task<AppUser> FindUserByName(string username)
        {
            return await _userManager.FindByNameAsync(username);

        }

        public async Task<IEnumerable<AppUserWithRoles>> GetUsersWithRoles()
        {
            var users = await _userManager.Users
                .OrderBy(u => u.UserName)
                .Select(u => new AppUserWithRoles
                {
                    Id = u.Id,
                    Username = u.UserName,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                })
                .ToListAsync();

            return users;
        }

        public async Task<IdentityResult> AddRoles(AppUser appUser, string[] selectedRoles, IList<string> userRoles)
        {
            return await _userManager.AddToRolesAsync(appUser, selectedRoles.Except(userRoles));
        }

        public async Task<IdentityResult> RemoveRoles(AppUser appUser, IList<string> userRoles, string[] selectedRoles)
        {
            return await _userManager.RemoveFromRolesAsync(appUser, userRoles.Except(selectedRoles));
        }
    }
}
