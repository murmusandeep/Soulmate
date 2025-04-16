using AutoMapper;
using Shared.Exceptions;
using Shared.Models;
using SoulmateBLL.Interfaces;
using SoulmateDAL.Interfaces;

namespace SoulmateBLL
{
    public class UsersBLL : IUsersBLL
    {
        private readonly IUsersDAL _usersDAL;
        private readonly IMapper _mapper;

        public UsersBLL(IUsersDAL usersDAL, IMapper mapper)
        {
            _usersDAL = usersDAL;
            _mapper = mapper;
        }

        public async Task<User> GetUser(string username)
        {
            var result = await _usersDAL.GetUser(username) ?? throw new UnAuthorizedException("Invalid Username");
            var user = _mapper.Map<User>(result);
            return user;
        }

        public async Task<User> GetUserById(int id)
        {
            var result = await _usersDAL.GetUserById(id) ?? throw new UserNotFoundByIdException(id);
            var user = _mapper.Map<User>(result);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var result = await _usersDAL.GetUsers();
            var users = _mapper.Map<IEnumerable<User>>(result);
            return users;
        }
    }
}
