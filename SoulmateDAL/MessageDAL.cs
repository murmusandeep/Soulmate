using Microsoft.EntityFrameworkCore;
using SoulmateDAL.Data;
using SoulmateDAL.Entities;
using SoulmateDAL.Interfaces;

namespace SoulmateDAL
{
    public class MessageDAL : IMessageDAL
    {
        private readonly DataContext _dataContext;

        public MessageDAL(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void AddGroup(Group group)
        {
            _dataContext.Groups.Add(group);
        }

        public void AddMessage(Message message)
        {
            _dataContext.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _dataContext.Messages.Remove(message);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _dataContext.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await _dataContext.Groups.Include(x => x.Connections).Where(x => x.Connections.Any(x => x.ConnectionId == connectionId)).FirstOrDefaultAsync();
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _dataContext.Messages.FindAsync(id);
        }

        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _dataContext.Groups
                .Include(x => x.Connections)
                .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public IQueryable<Message> GetMessagesForUser()
        {
            return _dataContext.Messages.OrderByDescending(x => x.MessageSent).AsQueryable();
        }

        public async Task<IEnumerable<Message>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            var messages = await _dataContext.Messages
                .Include(s => s.Sender).ThenInclude(p => p.Photos)
                .Include(r => r.Recipient).ThenInclude(p => p.Photos)
                .Where(
                    m => (m.RecipientUsername == currentUsername && m.RecipientDeleted == false && m.SenderUsername == recipientUsername) ||
                        (m.RecipientUsername == recipientUsername && m.SenderDeleted == false && m.SenderUsername == currentUsername)
                )
                .OrderBy(m => m.MessageSent)
                .ToListAsync();

            return messages;
        }

        public void RemoveConnection(Connection connection)
        {
            _dataContext.Connections.Remove(connection);
        }

        public void UpdateGroup(Group group)
        {
            var existingGroup = _dataContext.Groups
            .Include(g => g.Connections)
            .FirstOrDefault(g => g.Name == group.Name);

            if (existingGroup == null) throw new Exception("Group not found");

            foreach (var conn in group.Connections)
            {
                if (!existingGroup.Connections.Any(c => c.ConnectionId == conn.ConnectionId))
                {
                    existingGroup.Connections.Add(conn);
                }
            }
        }
    }
}
