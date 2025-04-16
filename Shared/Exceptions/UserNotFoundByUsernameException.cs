using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Exceptions
{
    public sealed class UserNotFoundByUsernameException : NotFoundException
    {
        public UserNotFoundByUsernameException(string username) : base($"The user with Username: {username} doesn't exist in the database.") { }
    }
}
