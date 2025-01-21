namespace Hermes.Core.Models;

public class Message
{
    public Guid Id { get; set; }

    public string? SenderId { get; set; }
    public User? Sender { get; set; }

    public Guid? PrivateChatId { get; set; }
    public PrivateChat? PrivateChat { get; set; }

    public Guid? GroupChatId { get; set; }
    public GroupChat? GroupChat { get; set; }

    public string? Content { get; set; }

    public DateTime SentAt { get; set; }
}