namespace Hermes.Core.Models;

public class GroupChat
{
    public Guid Id { get; set; }
    public string? Name { get; set; }

    public IList<GroupChatMembership> Memberships { get; set; } = [];
    public IList<Message> Messages { get; set; } = [];
}