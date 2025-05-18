using Microsoft.AspNetCore.Identity;
using SoulmateDAL.Entities;

namespace SoulmateDAL.Interfaces
{
    public interface IAdminDAL
    {
        Task<IEnumerable<AppUserWithRoles>> GetUsersWithRoles();
        Task<IList<string>> GetUserRoles(AppUser appUser);
        Task<AppUser> FindUserByName(string username);
        Task<IdentityResult> AddRoles(AppUser appUser, string[] selectedRoles, IList<string> userRoles);
        Task<IdentityResult> RemoveRoles(AppUser appUser, IList<string> userRoles, string[] selectedRoles);
    }
}
