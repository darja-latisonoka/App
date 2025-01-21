using Hermes.Core.Models;

namespace Hermes.Core.Interfaces;

public interface IChatService
{
    void AddPrivateMessage(string senderId, string receiverId, string content);
    PrivateChat GetPrivateChatOrCreateNew(string senderId, string receiverId);
    void AddGroupMessage(string senderId, string groupId, string content);
}