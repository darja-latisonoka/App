using Hermes.Core.Models;

namespace Hermes.API.Models;

public class GroupChatsViewModel
{
    public List<GroupChat> Chats { get; set; } = [];
    public List<User> OnlineUsers { get; set; } = [];
}