using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObject;
using Shared.Security;
using SoulmateBLL.Interfaces;

namespace Soulmate.Controllers
{
    public class AccountController : BaseApiController
    {

        private readonly IAccountBLL _accountBLL;
        private readonly ITokenHelper _tokenHelper;
        private readonly ILoggerManager _logger;

        public AccountController(IAccountBLL accountBLL, ITokenHelper tokenHelper, ILoggerManager logger)
        {
            _accountBLL = accountBLL;
            _tokenHelper = tokenHelper;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            _logger.LogInfo($"Attempting to register user: {register.Username}");

            var user = await _accountBLL.Register(register);

            _logger.LogInfo($"User registered successfully: {register.Username}");

            var userDto = new UserDto
            {
                Username = user.Username,
                Token = _tokenHelper.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };

            return Ok(userDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            _logger.LogInfo($"Login attempt for user: {loginDto.Username}");

            if (string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
                return BadRequest("Username and password must be provided.");

            var user = await _accountBLL.GetUser(loginDto.Username, loginDto.Password);

            _logger.LogInfo($"Login successful for user: {loginDto.Username}");

            var userDto = new UserDto
            {
                Username = user.Username,
                Token = _tokenHelper.CreateToken(user),
                PhotoUrl = user.PhotoUrl,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };

            return Ok(userDto);
        }
    }
}
