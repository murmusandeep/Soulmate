using Shared.DataTransferObject;
using Shared.Models;

namespace SoulmateBLL.Interfaces
{
    public interface IAccountBLL
    {
        Task<User> Register(RegisterDto register);
        Task<User> GetUser(string username, string password);
    }
}
