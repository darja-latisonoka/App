namespace Hermes.Core.Models;

public class GroupChatMembership
{
    public Guid Id { get; set; }

    public Guid? GroupChatId { get; set; }
    public GroupChat? GroupChat { get; set; }

    public string? UserId { get; set; }
    public User? User { get; set; }

    public bool? IsAdmin { get; set; }
}