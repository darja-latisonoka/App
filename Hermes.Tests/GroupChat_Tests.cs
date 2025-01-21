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
public class GroupChat_Tests
{
    private GroupChatController _controller;
    private Mock<IRepository<GroupChatMembership>> _groupChatMembershipRepositoryMock;
    private Mock<IRepository<GroupChat>> _groupChatRepositoryMock;
    private Mock<IRepository<Message>> _messageRepositoryMock;
    private Mock<UserManager<User>> _userManagerMock;

    [TestInitialize]
    public void Setup()
    {
        _groupChatRepositoryMock = new Mock<IRepository<GroupChat>>();
        _groupChatMembershipRepositoryMock = new Mock<IRepository<GroupChatMembership>>();
        _userManagerMock = new Mock<UserManager<User>>(new Mock<IUserStore<User>>().Object, null, null, null, null,
            null, null, null, null);
        _messageRepositoryMock = new Mock<IRepository<Message>>();

        _controller = new GroupChatController(
            _groupChatRepositoryMock.Object,
            _groupChatMembershipRepositoryMock.Object,
            _userManagerMock.Object,
            _messageRepositoryMock.Object);
    }

    [TestMethod]
    public void Index_ReturnsViewWithGroupChatsViewModel()
    {
        // Arrange
        var userId = "1";
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId)
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };

        var groupChats = new List<GroupChat>
        {
            new() { Id = Guid.NewGuid() },
            new() { Id = Guid.NewGuid() }
        };
        var onlineUsers = new List<User>
        {
            new() { Id = "1", IsOnline = true },
            new() { Id = "2", IsOnline = true }
        };

        _userManagerMock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
        _groupChatRepositoryMock.Setup(r => r.FindAll()).Returns(groupChats.AsQueryable());

        // Act
        var result = _controller.Index();

        // Assert
        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);

        var model = viewResult.Model as GroupChatsViewModel;
        Assert.IsNotNull(model);
    }
}