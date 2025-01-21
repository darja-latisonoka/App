using Hermes.API.Models;
using Hermes.Core.Interfaces;
using Hermes.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Controllers;

[Authorize]
[Route("[controller]")]
public class ChatController(
    UserManager<User> userManager,
    IChatService chatService,
    IRepository<User> userRepository) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var currentUser = await userManager.GetUserAsync(User);

        if (currentUser is null) return View("Error");

        var users = userRepository
            .FindByCondition(u => u.Id != currentUser.Id).ToList();

        return View(new ChatsViewModel
        {
            Users = users
        });
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> Chat(string username)
    {
        var interlocutor = await userManager.FindByNameAsync(username);

        if (interlocutor is null) return View("Error");

        var currentUser = await userManager.GetUserAsync(User);

        if (currentUser is null) return View("Error");

        var users = userRepository
            .FindByCondition(u => u.Id != currentUser.Id).ToList();

        var chat = chatService.GetPrivateChatOrCreateNew(currentUser.Id, interlocutor.Id);

        var sortedMessages = chat.Messages.OrderBy(m => m.SentAt).ToList();

        return View(new ChatViewModel
        {
            Chat = chat,
            Users = users,
            Interlocutor = interlocutor,
            Messages = sortedMessages
        });
    }
}