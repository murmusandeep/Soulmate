using Microsoft.AspNetCore.Identity;
using SoulmateDAL.Entities;

namespace SoulmateDAL.Interfaces
{
    public interface IAccountDAL
    {
        Task<IdentityResult> Register(AppUser user, string password);
        Task<bool> CheckUserValid(AppUser appUser, string password);
        Task<bool> UserExists(string username);
        Task<AppUser?> GetUser(string username);
        Task<IdentityResult> AddUserRole(AppUser appUser);
    }
}
