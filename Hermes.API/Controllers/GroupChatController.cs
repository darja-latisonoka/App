using System.Security.Claims;
using Hermes.API.Models;
using Hermes.Core.Interfaces;
using Hermes.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hermes.API.Controllers;

[Authorize]
[Route("[controller]")]
public class GroupChatController(
    IRepository<GroupChat> groupChatRepository,
    IRepository<GroupChatMembership> groupChatMembershipRepository,
    UserManager<User> userManager,
    IRepository<Message> messageRepository) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var chats = groupChatRepository.FindByCondition(c => c.Memberships.Any(m => m.UserId == userId)).ToList();

        var onlineUsers = userManager.Users.ToList().Where(u => u.IsOnline == true && u.Id != userId).ToList();

        return View(new GroupChatsViewModel
        {
            OnlineUsers = onlineUsers,
            Chats = chats
        });
    }

    [HttpPost]
    public IActionResult Create(List<string>? userIds, string? name)
    {
        var creatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIds is null || userIds.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "You must select at least one user.");

            return View("Index", new GroupChatsViewModel
            {
                OnlineUsers = userManager.Users.ToList().Where(u => u.IsOnline == true && u.Id != creatorId).ToList(),
                Chats = groupChatRepository.FindByCondition(c => c.Memberships.Any(m => m.UserId == creatorId))
                    .ToList()
            });
        }

        var distinctUserIds = userIds.Distinct().ToList();

        var memberships = distinctUserIds.Select(userId => new GroupChatMembership
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            IsAdmin = false
        }).ToList();

        memberships.Add(new GroupChatMembership
        {
            Id = Guid.NewGuid(),
            UserId = creatorId,
            IsAdmin = true
        });

        var chat = new GroupChat
        {
            Id = Guid.NewGuid(),
            Name = name,
            Memberships = memberships
        };

        groupChatRepository.Create(chat);

        var chats = groupChatRepository.FindByCondition(c => c.Memberships.Any(m => m.UserId == creatorId))
            .ToList();

        var onlineUsers = userManager.Users.ToList().Where(u => u.IsOnline == true && u.Id != creatorId).ToList();

        var currentUser = userManager.Users.FirstOrDefault(u => u.Id == creatorId)?.UserName;

        return View("Chat", new GroupChatViewModel
        {
            Id = chat.Id,
            GroupName = name ?? string.Empty,
            OnlineUsers = onlineUsers,
            Chats = chats,
            CurrentUser = currentUser ?? string.Empty
        });
    }

    [HttpGet("{id}")]
    public IActionResult Chat(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var chat = groupChatRepository.FindByCondition(c => c.Id == id).FirstOrDefault();

        if (chat is null) return NotFound();

        var chats = groupChatRepository.FindByCondition(c => c.Memberships.Any(m => m.UserId == userId))
            .ToList();

        var onlineUsers = userManager.Users.ToList().Where(u => u.IsOnline == true && u.Id != userId).ToList();

        var messages = messageRepository.FindByCondition(m => m.GroupChatId == id).OrderBy(m => m.SentAt).ToList();

        return View(new GroupChatViewModel
        {
            Id = id,
            GroupName = chat.Name ?? string.Empty,
            OnlineUsers = onlineUsers,
            Chats = chats,
            Messages = messages,
            CurrentUserId = userId ?? string.Empty,
            CurrentUser = userManager.Users.FirstOrDefault(u => u.Id == userId)?.UserName ?? string.Empty
        });
    }

    [HttpGet("{id}/info")]
    public IActionResult Info(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var chat = groupChatRepository.FindByCondition(c => c.Id == id).Include(c => c.Memberships).FirstOrDefault();

        if (chat is null) return NotFound();

        var chats = groupChatRepository.FindByCondition(c => c.Memberships.Any(m => m.UserId == userId))
            .ToList();

        var onlineUsers = userManager.Users.ToList().Where(u => u.IsOnline == true && u.Id != userId).ToList();

        var members = chat.Memberships.Select(m => new MemberViewModel
        {
            UserName = m.User?.UserName ?? string.Empty,
            IsAdmin = m.IsAdmin ?? false
        }).ToList();


        return View("Info", new GroupChatInfoViewModel
        {
            Id = id,
            Name = chat.Name ?? string.Empty,
            OnlineUsers = onlineUsers,
            Chats = chats,
            Members = members
        });
    }

    [HttpPost("leave")]
    public IActionResult LeaveGroupChat(Guid groupId, string userId)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var membership = groupChatMembershipRepository
            .FindByCondition(m => m.GroupChatId == groupId && m.UserId == currentUserId)
            .FirstOrDefault();

        if (membership == null) return BadRequest("You are not a member of this group.");

        bool? isAdmin = membership.IsAdmin;

        // Remove the membership
        groupChatMembershipRepository.Delete(membership);

        // Check if the group has any remaining members
        var remainingMembers = groupChatMembershipRepository
            .FindByCondition(m => m.GroupChatId == groupId)
            .ToList();

        if (remainingMembers.Count == 0)
        {
            var groupChat = groupChatRepository
                .FindByCondition(g => g.Id == groupId)
                .FirstOrDefault();
            if (groupChat != null) groupChatRepository.Delete(groupChat);
        }
        else if (isAdmin == true)
        {
            var random = new Random();
            var newAdmin = remainingMembers[random.Next(remainingMembers.Count)];
            newAdmin.IsAdmin = true;
            groupChatMembershipRepository.Update(newAdmin);
        }

        return RedirectToAction("Index");
    }

}