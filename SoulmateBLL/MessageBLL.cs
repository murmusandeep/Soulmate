using AutoMapper;
using AutoMapper.QueryableExtensions;
using Shared.DataTransferObject;
using Shared.Exceptions;
using Shared.Helper;
using SoulmateBLL.Interfaces;
using SoulmateDAL.Entities;
using SoulmateDAL.Interfaces;

namespace SoulmateBLL
{
    public class MessageBLL : IMessageBLL
    {
        private readonly IUsersDAL _usersDAL;
        private readonly IMessageDAL _messageDAL;
        private readonly IMapper _mapper;

        public MessageBLL(IUsersDAL usersDAL, IMessageDAL messageDAL, IMapper mapper)
        {
            _usersDAL = usersDAL;
            _messageDAL = messageDAL;
            _mapper = mapper;
        }
        public async Task<MessageDto> AddMessage(string username, CreateMessageDto createMessageDto)
        {
            if (username == createMessageDto.RecipientUsername.ToLower())
                throw new BadRequestException("You cannot send message to yourself");

            var sender = await _usersDAL.GetUserByUsername(username);
            var recipent = await _usersDAL.GetUserByUsername(createMessageDto.RecipientUsername);

            if (recipent == null)
                throw new UserNotFoundException(createMessageDto.RecipientUsername);

            var message = new Message
            {
                Sender = sender,
                SenderUsername = sender.UserName,
                Recipient = recipent,
                RecipientUsername = recipent.UserName,
                Content = createMessageDto.Content,
            };

            _messageDAL.AddMessage(message);

            if (await _usersDAL.SaveAllAsync()) return _mapper.Map<MessageDto>(message);

            throw new BadRequestException("Failed to send message");
        }

        public async Task<bool> DeleteMessage(string username, int id)
        {
            var message = await _messageDAL.GetMessage(id);

            if (message == null)
                throw new BadRequestException("Cannot delete this message");

            if (message.SenderUsername != username && message.RecipientUsername != username)
                throw new UnAuthorizedException("Unauthorize");

            if (message.SenderUsername == username) message.SenderDeleted = true;
            if (message.RecipientUsername == username) message.RecipientDeleted = true;

            if (message is { SenderDeleted: true, RecipientDeleted: true })
            {
                _messageDAL.DeleteMessage(message);
            }

            return await _usersDAL.SaveAllAsync();
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {
            var query = _messageDAL.GetMessagesForUser();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.RecipientUsername == messageParams.Username && u.RecipientDeleted == false),
                "Outbox" => query.Where(u => u.SenderUsername == messageParams.Username && u.SenderDeleted == false),
                _ => query.Where(u => u.RecipientUsername == messageParams.Username && u.RecipientDeleted == false && u.DateRead == null)
            };

            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string username)
        {
            var result = await _messageDAL.GetMessageThread(currentUsername, username);

            var unreadMessages = result.Where(m => m.DateRead == null && m.RecipientUsername == currentUsername).ToList();

            if (unreadMessages.Any())
            {
                foreach (var message in unreadMessages)
                {
                    message.DateRead = DateTime.UtcNow;
                }

                await _usersDAL.SaveAllAsync();
            }

            return _mapper.Map<IEnumerable<MessageDto>>(result);
        }
    }
}
