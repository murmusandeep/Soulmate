using SoulmateDAL.Entities;

namespace SoulmateDAL.Interfaces
{
    public interface IUsersDAL
    {
        Task<IEnumerable<AppUser>> GetUsers();
        Task<AppUser> GetUserById(int id);
        Task<AppUser> GetUser(string username);
    }
}
