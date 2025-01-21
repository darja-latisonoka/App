using Hermes.Core.Models;

namespace Hermes.API.Models;

public class GroupChatInfoViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<MemberViewModel> Members { get; set; } = [];
    public List<User> OnlineUsers { get; set; } = [];
    public List<GroupChat> Chats { get; set; } = [];
    public string CurrentUserId { get; set; } = string.Empty;
}