using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Hermes.API.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Hermes.Core.Models;
using Hermes.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HermesTests
{
    [TestClass]
    public class UpdateProfile_Tests
    {
        private Mock<ILogger<HomeController>> _mockLogger;
        private Mock<UserManager<User>> _mockUserManager;
        private HomeController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();

            var userStoreMock = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            _controller = new HomeController(_mockLogger.Object, _mockUserManager.Object);
        }

        [TestMethod]
        public async Task UpdateProfile_UserNameAlreadyTaken_ReturnsProfileViewWithModelError()
        {
            // Arrange
            var user = new User { UserName = "currentusername", Email = "currentemail@example.com" };
            var existingUserWithUsername = new User { UserName = "newusername" };

            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);
            _mockUserManager.Setup(um => um.FindByNameAsync("newusername"))
                .ReturnsAsync(existingUserWithUsername);

            var model = new ProfileViewModel
            {
                UserName = "newusername",
                Email = "currentemail@example.com"
            };

            // Act
            var result = await _controller.Update(model) as ViewResult;

            // Assert
            Assert.AreEqual("Profile", result.ViewName);
            Assert.IsTrue(result.ViewData.ModelState["UserName"].Errors.Count > 0);
            Assert.AreEqual("Username is already taken.", result.ViewData.ModelState["UserName"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public async Task UpdateProfile_EmailAlreadyTaken_ReturnsProfileViewWithModelError()
        {
            // Arrange
            var user = new User { UserName = "currentusername", Email = "currentemail@example.com" };
            var existingUserWithEmail = new User { Email = "newemail@example.com" };

            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync(user);
            _mockUserManager.Setup(um => um.FindByEmailAsync("newemail@example.com"))
                .ReturnsAsync(existingUserWithEmail);

            var model = new ProfileViewModel
            {
                UserName = "currentusername", // Username remains the same
                Email = "newemail@example.com"  // Changing the email to one that's already taken
            };

            // Act
            var result = await _controller.Update(model) as ViewResult;

            // Assert
            Assert.AreEqual("Profile", result.ViewName);
            Assert.IsTrue(result.ViewData.ModelState["Email"].Errors.Count > 0);
            Assert.AreEqual("Email is already in use.", result.ViewData.ModelState["Email"].Errors[0].ErrorMessage);
        }






    }
}
