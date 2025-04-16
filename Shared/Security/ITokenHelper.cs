using Shared.Models;

namespace Shared.Security
{
    public interface ITokenHelper
    {
        string CreateToken(User user);
    }
}
