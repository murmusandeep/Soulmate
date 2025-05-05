using Microsoft.AspNetCore.Mvc;
using Shared.Helper;
using Soulmate.Extensions;
using SoulmateBLL.Interfaces;

namespace Soulmate.Controllers
{
    public class LikesController : BaseApiController
    {
        private readonly ILikesBLL _likesBLL;

        public LikesController(ILikesBLL likesBLL)
        {
            _likesBLL = likesBLL;
        }

        [HttpPost("{targetUserId:int}")]
        public async Task<IActionResult> ToggleLike(int targetUserId)
        {
            var sourceUserId = User.GetUserId();
            if (await _likesBLL.ToggleLike(sourceUserId, targetUserId)) return Ok();
            return BadRequest("Failed to like user");
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetCurrentUserLikeIds()
        {
            return Ok(await _likesBLL.GetCurrentUserLikeIds(User.GetUserId()));
        }

        [HttpGet]
        public async Task<IActionResult> GetUserLikes([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await _likesBLL.GetUserLikes(likesParams);
            Response.AddPaginationHeader(users);
            return Ok(users);
        }
    }
}
