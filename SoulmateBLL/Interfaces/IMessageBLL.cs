using Shared.DataTransferObject;
using Shared.Helper;

namespace SoulmateBLL.Interfaces
{
    public interface IMessageBLL
    {
        Task<MessageDto> AddMessage(string username, CreateMessageDto createMessageDto);
        Task<bool> DeleteMessage(string username, int id);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string username);
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);
    }
}
