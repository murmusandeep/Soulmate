using Shared.Models;

namespace SoulmateBLL.Interfaces
{
    public interface IUsersBLL
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(int id);
        Task<User> GetUser(string username);
    }
}
