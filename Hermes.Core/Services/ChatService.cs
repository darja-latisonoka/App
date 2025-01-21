using Hermes.Core.Interfaces;
using Hermes.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Core.Services;

public class ChatService(IRepository<PrivateChat> privateChatRepository, IRepository<Message> messageRepository)
    : IChatService
{
    public void AddPrivateMessage(string senderId, string receiverId, string content)
    {
        ArgumentNullException.ThrowIfNull(senderId);
        ArgumentNullException.ThrowIfNull(receiverId);
        ArgumentNullException.ThrowIfNull(content);

        var privateChatId = GetPrivateChatOrCreateNew(senderId, receiverId).Id;

        messageRepository.Create(new Message
        {
            Id = Guid.NewGuid(),
            PrivateChatId = privateChatId,
            SenderId = senderId,
            Content = content,
            SentAt = DateTime.UtcNow
        });
    }

    public PrivateChat GetPrivateChatOrCreateNew(string senderId, string receiverId)
    {
        var chat = privateChatRepository
            .FindByCondition(chat =>
                (chat.User1Id == senderId && chat.User2Id == receiverId) ||
                (chat.User1Id == receiverId && chat.User2Id == senderId))
            .Include(chat => chat.Messages)
            .ToList()
            .FirstOrDefault();

        if (chat is not null) return chat;

        return privateChatRepository.Create(new PrivateChat
        {
            Id = Guid.NewGuid(),
            User1Id = senderId,
            User2Id = receiverId
        });
    }

    public void AddGroupMessage(string senderId, string groupId, string content)
    {
        ArgumentNullException.ThrowIfNull(senderId);
        ArgumentNullException.ThrowIfNull(groupId);
        ArgumentNullException.ThrowIfNull(content);

        messageRepository.Create(new Message
        {
            Id = Guid.NewGuid(),
            GroupChatId = new Guid(groupId),
            SenderId = senderId,
            Content = content,
            SentAt = DateTime.UtcNow
        });
    }
}