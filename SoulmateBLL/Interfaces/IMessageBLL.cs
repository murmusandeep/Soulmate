using Shared.DataTransferObject;
using Shared.Helper;

namespace SoulmateBLL.Interfaces
{
    public interface IMessageBLL
    {
        Task<(bool Success, MessageDto Message)> AddMessage(string username, CreateMessageDto createMessageDto);
        Task<bool> DeleteMessage(string username, int id);
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string username);
        Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams);
        Task<GroupDto> GetMessageGroup(string groupName);
        void AddGroup(GroupDto groupDto);
        Task<ConnectionDto> GetConnection(string connectionId);
        void RemoveConnection(ConnectionDto connectionDto);
        void UpdateGroup(GroupDto groupDto);
        Task<bool> SaveAllAsync();
    }
}
