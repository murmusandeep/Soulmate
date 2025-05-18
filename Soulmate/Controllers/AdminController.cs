using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoulmateBLL.Interfaces;

namespace Soulmate.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly IAdminBLL _adminBLL;
        private readonly IPhotoBLL _photoBLL;

        public AdminController(IAdminBLL adminBLL, IPhotoBLL photoBLL)
        {
            _adminBLL = adminBLL;
            _photoBLL = photoBLL;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var users = await _adminBLL.GetUsersWithRoles();

            return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<IActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            if (string.IsNullOrEmpty(roles)) return BadRequest("You must select atleast one role");
            var result = await _adminBLL.EditUserRoles(username, roles);
            return Ok(result);
        }

        //[Authorize(Policy = "ModeratePhotoRole")]
        //[HttpGet("photos-to-moderate")]
        //public async Task<IActionResult> GetPhotosForModeration()
        //{
        //    var photos = await _photoBLL.GetUnapprovedPhotos();
        //    return Ok(photos);
        //}

        //[Authorize(Policy = "ModeratePhotoRole")]
        //[HttpPost("approve-photo/{photoId}")]
        //public async Task<IActionResult> ApprovePhoto(int photoId)
        //{
        //    var result = await _photoBLL.ApprovePhoto(photoId);
        //    if (!result) return BadRequest("Could not able to Approve Photo");
        //    return Ok();
        //}

        //[Authorize(Policy = "ModeratePhotoRole")]
        //[HttpPost("reject-photo/{photoId}")]
        //public async Task<IActionResult> RejectPhoto(int photoId)
        //{
        //    var result = await _photoBLL.RemovePhoto(photoId);
        //    if (!result) return BadRequest("Could not able to Reject Photo");
        //    return Ok();
        //}
    }
}
