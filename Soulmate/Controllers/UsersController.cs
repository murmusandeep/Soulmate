using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoulmateBLL.Interfaces;

namespace Soulmate.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUsersBLL _usersBLL;

        public UsersController(IUsersBLL usersBLL)
        {
            _usersBLL = usersBLL;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _usersBLL.GetUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _usersBLL.GetUserById(id);
            return Ok(user);
        }

        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _usersBLL.GetUser(username);
            return Ok(user);
        }
    }
}
