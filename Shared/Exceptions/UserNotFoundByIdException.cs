using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Exceptions
{
    public sealed class UserNotFoundByIdException : NotFoundException
    {
        public UserNotFoundByIdException(int id) : base($"The user with Id: {id} doesn't exist in the database.") { }
    }
}
