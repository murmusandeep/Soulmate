using AutoMapper;
using Shared.DataTransferObject;
using Shared.Exceptions;
using SoulmateBLL.Interfaces;
using SoulmateDAL.Interfaces;

namespace SoulmateBLL
{
    public class AdminBLL : IAdminBLL
    {
        private readonly IAdminDAL _adminDAL;
        private readonly IMapper _mapper;

        public AdminBLL(IAdminDAL adminDAL, IMapper mapper)
        {
            _adminDAL = adminDAL;
            _mapper = mapper;
        }

        public async Task<IList<string>> EditUserRoles(string username, string roles)
        {
            var selectedRoles = roles.Split(",").ToArray();

            var user = await _adminDAL.FindUserByName(username);

            if (user == null)
                throw new UserNotFoundException(username);

            var userRoles = await _adminDAL.GetUserRoles(user);

            var result = await _adminDAL.AddRoles(user, selectedRoles, userRoles);

            if (!result.Succeeded)
                throw new BadRequestException("Failed to add to roles");

            result = await _adminDAL.RemoveRoles(user, userRoles, selectedRoles);

            if (!result.Succeeded)
                throw new BadRequestException("Failed to remove from role");

            return await _adminDAL.GetUserRoles(user);
        }

        public async Task<IEnumerable<UserWithRolesDto>> GetUsersWithRoles()
        {
            var results = await _adminDAL.GetUsersWithRoles();

            var users = _mapper.Map<IEnumerable<UserWithRolesDto>>(results);
            return users;
        }
    }
}
