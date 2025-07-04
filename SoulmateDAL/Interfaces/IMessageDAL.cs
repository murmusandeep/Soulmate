﻿using SoulmateDAL.Entities;

namespace SoulmateDAL.Interfaces
{
    public interface IMessageDAL
    {
        void AddMessage(Message message);
        void DeleteMessage(Message message);
        Task<Message> GetMessage(int id);
        IQueryable<Message> GetMessagesForUser();
        Task<IEnumerable<Message>> GetMessageThread(string currentUsername, string recipientUsername);
        void AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection> GetConnection(string connectionId);
        void UpdateGroup(Group group);
        Task<Group> GetMessageGroup(string groupName);
        Task<Group> GetGroupForConnection(string connectionId);
    }
}
