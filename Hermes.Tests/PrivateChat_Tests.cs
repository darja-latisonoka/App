using System.Linq.Expressions;
using System.Security.Claims;
using Hermes.API.Controllers;
using Hermes.API.Models;
using Hermes.Core.Interfaces;
using Hermes.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hermes.Tests;

[TestClass]
public class PrivateChat_Tests
{
    private Mock<IChatService> _chatService;
    private ChatController _controller;
    private Mock<UserManager<User>> _userManager;
    private Mock<IRepository<User>> _userRepository;

    [TestInitialize]
    public void Setup()
    {
        _chatService = new Mock<IChatService>();
        _userManager = new Mock<UserManager<User>>(new Mock<IUserStore<User>>().Object, null, null, null, null,
            null, null, null, null);
        _userRepository = new Mock<IRepository<User>>();

        _controller = new ChatController(_userManager.Object, _chatService.Object, _userRepository.Object);
    }

    [TestMethod]
    public async void Index_ReturnsViewWithPrivateChatsViewModel()
    {
        // Arrange
        var userId = "1";
        var userName = "JohnDoe";
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Name, userName)
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };

        var privateChats = new List<PrivateChat>
        {
            new() { Id = Guid.NewGuid() },
            new() { Id = Guid.NewGuid() }
        };
        var users = new List<User>
        {
            new() { Id = "1", IsOnline = true },
            new() { Id = "2", IsOnline = true }
        };

        _userRepository.Setup(r => r.FindByCondition(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns(users.AsQueryable());

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);

        var model = viewResult.Model as ChatsViewModel;
        Assert.IsNotNull(model);
    }
}