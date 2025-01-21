using Microsoft.AspNetCore.Identity;

namespace Hermes.Core.Models;

public class User : IdentityUser
{
    public bool? IsOnline { get; set; }

    public IList<GroupChatMembership> GroupChatMemberships { get; set; } = [];
}