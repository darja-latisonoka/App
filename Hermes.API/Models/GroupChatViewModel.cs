using Hermes.Core.Models;

namespace Hermes.API.Models;

public class GroupChatViewModel
{
    public string GroupName { get; set; } = string.Empty;
    public List<GroupChat> Chats { get; set; } = [];
    public List<User> OnlineUsers { get; set; } = [];
    public List<Message> Messages { get; set; } = [];
    public string CurrentUserId { get; set; } = string.Empty;
    public Guid Id { get; set; }
    public string CurrentUser { get; set; } = string.Empty;
}