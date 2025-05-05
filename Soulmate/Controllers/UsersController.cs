using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObject;
using Shared.Helper;
using Soulmate.Extensions;
using SoulmateBLL.Interfaces;

namespace Soulmate.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUsersBLL _usersBLL;
        private readonly IPhotoBLL _photoBLL;

        public UsersController(IUsersBLL usersBLL, IPhotoBLL photoBLL)
        {
            _usersBLL = usersBLL;
            _photoBLL = photoBLL;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserParams userParams)
        {
            userParams.CurrentUsername = User.GetUsername();
            var users = await _usersBLL.GetUsers(userParams);
            Response.AddPaginationHeader(users);
            return Ok(users);
        }

        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _usersBLL.GetUser(username);
            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(MemberUpdateDto member)
        {
            var result = await _usersBLL.UpdateUser(member, User.GetUsername());
            if (result) return NoContent();
            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<IActionResult> AddPhoto(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided or file is empty.");

            if (!file.ContentType.StartsWith("image/"))
                return BadRequest("Only image files are allowed.");

            if (file.Length > 5 * 1024 * 1024) // 5MB limit
                return BadRequest("File size exceeds the 5MB limit.");

            var result = await _photoBLL.AddPhotoAsync(User.GetUsername(), file);
            return CreatedAtAction(nameof(GetUserByUsername), new { username = User.GetUsername() }, result);
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            if (await _photoBLL.DeletePhotoAsync(User.GetUsername(), photoId)) return NoContent();
            return BadRequest("Problem deleting photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<IActionResult> SetMainPhoto(int photoId)
        {
            if (await _photoBLL.SetMainPhoto(User.GetUsername(), photoId)) return NoContent();
            return BadRequest("Problem setting main photo");
        }
    }
}
