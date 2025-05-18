using AutoMapper;
using LoggerService;
using Shared.DataTransferObject;
using Shared.Exceptions;
using Shared.Security;
using SoulmateBLL.Interfaces;
using SoulmateDAL.Entities;
using SoulmateDAL.Interfaces;

namespace SoulmateBLL
{
    public class AccountBLL : IAccountBLL
    {
        private readonly IAccountDAL _accountDAL;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILoggerManager _logger;

        public AccountBLL(IAccountDAL accountDAL, IMapper mapper, IPasswordHasher passwordHasher, ILoggerManager logger)
        {
            _accountDAL = accountDAL;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<MemberDto> Register(RegisterDto register)
        {
            if (await _accountDAL.UserExists(register.Username))
            {
                _logger.LogWarn($"Registration failed: Username already exists - {register.Username}");
                throw new Exception("Username already exists.");
            }

            var appUser = _mapper.Map<AppUser>(register);

            appUser.UserName = register.Username.ToLower();

            var result = await _accountDAL.Register(appUser, register.password);

            if (!result.Succeeded)
                throw new BadRequestException("Unable to Register");

            var roleResult = await _accountDAL.AddUserRole(appUser);

            if (!roleResult.Succeeded)
                throw new BadRequestException("Unable to Add Role");

            _logger.LogInfo($"User created: {register.Username}");

            return _mapper.Map<MemberDto>(appUser);
        }

        public async Task<MemberDto> GetUser(string username, string password)
        {
            var appUser = await _accountDAL.GetUser(username);

            if (appUser == null)
                throw new UnAuthorizedException("Invalid username or password.");

            var result = await _accountDAL.CheckUserValid(appUser, password);

            if (!result)
                throw new UnAuthorizedException("Invalid username or password.");

            return _mapper.Map<MemberDto>(appUser);
        }
    }
}
