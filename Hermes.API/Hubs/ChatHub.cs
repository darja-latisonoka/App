using System.Security.Claims;
using Hermes.Core.Interfaces;
using Hermes.Core.Models;
using Microsoft.AspNetCore.Identity;
using ClientProxyExtensions = Microsoft.AspNetCore.SignalR.ClientProxyExtensions;
using Hub = Microsoft.AspNetCore.SignalR.Hub;

namespace Hermes.API.Hubs;

public class ChatHub(
    IChatService chatService,
    UserManager<User> userManager,
    IRepository<GroupChatMembership> groupChatMembershipRepository) : Hub
{
    public async Task SendMessage(string userId, string message)
    {
        var senderId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException();

        chatService.AddPrivateMessage(senderId, userId, message);

        await ClientProxyExtensions.SendAsync(Clients.User(userId), "ReceiveMessage", senderId, message);
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException();

        var user = userManager.FindByIdAsync(userId).Result ?? throw new InvalidOperationException();

        user.IsOnline = true;

        await userManager.UpdateAsync(user);

        await ClientProxyExtensions.SendAsync(Clients.All, "ChangedOnlineStatus", userId, true);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException();

        var user = userManager.FindByIdAsync(userId).Result ?? throw new InvalidOperationException();

        user.IsOnline = false;

        await userManager.UpdateAsync(user);

        await ClientProxyExtensions.SendAsync(Clients.All, "ChangedOnlineStatus", userId, false);

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendGroupMessage(string groupId, string message)
    {
        var senderId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException();

        var username = Context.User?.FindFirstValue(ClaimTypes.Name) ?? throw new InvalidOperationException();

        chatService.AddGroupMessage(senderId, groupId, message);

        var userIds = GetGroupMemberIds(groupId);

        userIds.Remove(senderId);

        await ClientProxyExtensions.SendAsync(Clients.Users(userIds), "ReceiveGroupMessage", groupId, username,
            message);
    }

    public List<string> GetGroupMemberIds(string groupId)
    {
        return groupChatMembershipRepository
            .FindByCondition(membership => membership.GroupChatId == new Guid(groupId))
            .Select(membership => membership.UserId ?? string.Empty)
            .ToList();
    }
}