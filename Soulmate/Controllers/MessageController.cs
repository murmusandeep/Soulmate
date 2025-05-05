using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObject;
using Shared.Helper;
using Soulmate.Extensions;
using SoulmateBLL.Interfaces;

namespace Soulmate.Controllers
{
    [Authorize]
    public class MessageController : BaseApiController
    {
        private readonly IMessageBLL _messageBLL;

        public MessageController(IMessageBLL messageBLL)
        {
            _messageBLL = messageBLL;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();
            var result = await _messageBLL.AddMessage(username, createMessageDto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();
            var messages = await _messageBLL.GetMessagesForUser(messageParams);
            Response.AddPaginationHeader(messages);
            return Ok(messages);
        }

        [HttpGet("thread/{username}")]
        public async Task<IActionResult> GetMessageThread(string username)
        {
            var currentUsername = User.GetUsername();

            return Ok(await _messageBLL.GetMessageThread(currentUsername, username));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var username = User.GetUsername();
            if (await _messageBLL.DeleteMessage(username, id)) return Ok();
            return BadRequest("Problem deleting message");
        }
    }
}
