using SoulmateDAL.Entities;

namespace SoulmateDAL.Interfaces
{
    public interface IAccountDAL
    {
        Task Register(AppUser appUser);
        Task<bool> UserExists(string username);
        Task<AppUser> GetUser(string username);
    }
}
