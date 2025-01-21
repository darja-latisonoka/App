using Hermes.Core.Models;

namespace Hermes.API.Models;

public class ChatViewModel
{
    public List<User> Users { get; set; } = [];
    public User Interlocutor { get; set; }
    public PrivateChat Chat { get; set; }
    public List<Message> Messages { get; set; } = [];
}