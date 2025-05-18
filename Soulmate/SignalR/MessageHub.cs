using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Shared.DataTransferObject;
using Soulmate.Extensions;
using SoulmateBLL.Interfaces;
using SoulmateDAL.Entities;
using SoulmateDAL.Interfaces;

namespace Soulmate.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IMessageBLL _messageBLL;
        private readonly IMessageDAL _messageDAL;
        private readonly IUsersDAL _usersDAL;
        private readonly IMapper _mapper;
        private readonly IHubContext<PresenceHub> _presenceHub;

        public MessageHub(IMessageBLL messageBLL, IMessageDAL messageDAL, IUsersDAL usersDAL, IMapper mapper, IHubContext<PresenceHub> presenceHub)
        {
            _messageBLL = messageBLL;
            _messageDAL = messageDAL;
            _usersDAL = usersDAL;
            _mapper = mapper;
            _presenceHub = presenceHub;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext?.Request.Query["user"];

            if (Context.User == null || string.IsNullOrEmpty(otherUser)) throw new Exception("Cannot join group");
            var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            var group = await AddToGroup(groupName);

            await Clients.Group(groupName).SendAsync("UpdatedGroup", group);

            var messages = await _messageBLL.GetMessageThread(Context.User.GetUsername(), otherUser!);

            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var group = await RemoveFromMessageGroup();
            await Clients.Group(group.Name).SendAsync("UpdatedGroup");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(CreateMessageDto createMessageDto)
        {
            if (Context.User == null) throw new Exception("could not get user");
            var username = Context.User.GetUsername();

            if (username == createMessageDto.RecipientUsername.ToLower())
                throw new HubException("You cannot send message to yourself");

            var sender = await _usersDAL.GetUserByUsername(username);
            var recipient = await _usersDAL.GetUserByUsername(createMessageDto.RecipientUsername);

            if (sender == null || recipient == null || sender.UserName == null || recipient.UserName == null)
                throw new HubException("cannot send message at this time");

            var message = new Message
            {
                Sender = sender,
                SenderUsername = sender.UserName,
                Recipient = recipient,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content,
            };

            var groupName = GetGroupName(sender.UserName, recipient.UserName);
            var group = await _messageDAL.GetMessageGroup(groupName);

            if (group != null && group.Connections.Any(x => x.Username == recipient.UserName))
            {
                message.DateRead = DateTime.UtcNow;
            }
            else
            {
                var connections = await PresenceTracker.GetConnectionsForUser(recipient.UserName);
                if (connections != null && connections?.Count != null)
                {
                    await _presenceHub.Clients.Clients(connections).SendAsync("NewMessageRecieved", new { username = sender.UserName, knownAs = sender.KnownAs });
                }
            }

            _messageDAL.AddMessage(message);

            if (await _usersDAL.SaveAllAsync())
            {
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }
        }

        private async Task<Group> AddToGroup(string groupName)
        {
            if (Context.User == null) throw new Exception("could not get user");
            var username = Context.User.GetUsername();

            var group = await _messageDAL.GetMessageGroup(groupName);
            var connection = new Connection { ConnectionId = Context.ConnectionId, Username = Context.User.GetUsername() };

            if (group == null)
            {
                group = new Group { Name = groupName };
                _messageDAL.AddGroup(group);
            }

            group.Connections.Add(connection);

            if (await _usersDAL.SaveAllAsync()) return group;

            throw new HubException("Failed to add to group");
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await _messageDAL.GetGroupForConnection(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);

            if (connection != null && group != null)
            {
                _messageDAL.RemoveConnection(connection);
                if (await _usersDAL.SaveAllAsync()) return group;
            }

            throw new HubException("Failed to remove from group");
        }

        private string GetGroupName(string caller, string? other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0;

            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}
