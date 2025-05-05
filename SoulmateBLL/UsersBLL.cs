using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Shared.DataTransferObject;
using Shared.Exceptions;
using Shared.Helper;
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

        public async Task<MemberDto> GetUser(string username)
        {
            var result = _usersDAL.GetUserAsync(username) ?? throw new UnAuthorizedException("Invalid Username");

            var user = await result
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<PagedList<MemberDto>> GetUsers(UserParams userParams)
        {
            var users = _usersDAL.GetUsers();

            users = users.Where(x => x.UserName != userParams.CurrentUsername);

            if (userParams.Gender != null)
            {
                users = users.Where(x => x.Gender == userParams.Gender);
            }

            var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
            var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

            users = users.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            users = userParams.OrderBy switch
            {
                "created" => users.OrderByDescending(u => u.Created),
                _ => users.OrderByDescending(u => u.LastActive)
            };

            return await PagedList<MemberDto>.CreateAsync(users.ProjectTo<MemberDto>(_mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> UpdateUser(MemberUpdateDto memberUpdateDto, string username)
        {
            var result = await _usersDAL.GetUserByUsername(username);
            if (result is null)
                throw new UserNotFoundException(username);
            _mapper.Map(memberUpdateDto, result);
            return await _usersDAL.SaveAllAsync();
        }
    }
}
