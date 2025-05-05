using SoulmateDAL.Entities;

namespace SoulmateDAL.Interfaces
{
    public interface IUsersDAL
    {
        IQueryable<AppUser> GetUsers();
        Task<AppUser> GetUserByUsername(string username);
        Task<AppUser> GetUserById(int id);
        IQueryable<AppUser> GetUserAsync(string username);
        Task<bool> SaveAllAsync();
    }
}
