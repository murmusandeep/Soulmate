using Shared.DataTransferObject;

namespace SoulmateBLL.Interfaces
{
    public interface IAccountBLL
    {
        Task<MemberDto> Register(RegisterDto register);
        Task<MemberDto> GetUser(string username, string password);
    }
}
