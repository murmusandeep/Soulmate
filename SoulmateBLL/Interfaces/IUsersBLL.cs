using Shared.DataTransferObject;
using Shared.Helper;

namespace SoulmateBLL.Interfaces
{
    public interface IUsersBLL
    {
        Task<PagedList<MemberDto>> GetUsers(UserParams userParams);
        Task<MemberDto> GetUser(string username);
        Task<bool> UpdateUser(MemberUpdateDto memberUpdateDto, string username);
    }
}
