using Shared.DataTransferObject;

namespace Shared.Security
{
    public interface ITokenHelper
    {
        Task<string> CreateToken(MemberDto user);
    }
}
