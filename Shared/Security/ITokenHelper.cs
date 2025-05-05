using Shared.DataTransferObject;

namespace Shared.Security
{
    public interface ITokenHelper
    {
        string CreateToken(MemberDto user);
    }
}
