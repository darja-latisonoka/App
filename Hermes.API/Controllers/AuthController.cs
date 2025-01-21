using Hermes.API.Models;
using Hermes.Core.Interfaces;
using Hermes.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.API.Controllers;

public class AuthController(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IRepository<User> userRepository,
    ILogger<AuthController> logger,
    IEmailSender emailSender)
    : Controller
{
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        if (userRepository.FindByCondition(user => user.UserName == model.Username).Any())
        {
            ModelState.AddModelError(string.Empty, "Username already taken.");
            return View(model);
        }

        if (userRepository.FindByCondition(user => user.Email == model.Email).Any())
        {
            ModelState.AddModelError(string.Empty, "Email already taken.");
            return View(model);
        }

        var user = new User { UserName = model.Username, Email = model.Email };

        var result = await userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Chat");
        }

        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

        return View(model);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();


        // Redirect to the home page or login page
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

        if (result.Succeeded)
        {
            var user = await userManager.FindByNameAsync(model.Username);

            if (user?.UserName is null || user.Email is null)
            {
                logger.LogError("User account {Username} not found.", model.Username);
                throw new InvalidOperationException("User account not found.");
            }

            var profileModel = new ProfileViewModel
            {
                UserName = user.UserName,
                Email = user.Email
            };

            return RedirectToAction("Index", "Chat",
                new { userName = profileModel.UserName, email = profileModel.Email });
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(string.Empty, "User account locked out.");

            logger.LogWarning("User account {Username} locked out.", model.Username);

            return View(model);
        }

        ModelState.AddModelError(string.Empty, "Incorrect username or password.");

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return NotFound();

        // Delete the user
        var result = await userManager.DeleteAsync(user);

        if (result.Succeeded)
        {
            // Sign out the user
            await signInManager.SignOutAsync();

            // Redirect to home or login page
            TempData["SuccessMessage"] = "Your account has been deleted successfully.";
            return RedirectToAction("Index", "Home"); // Adjust as needed
        }

        // Handle errors
        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

        // Return to the profile page with errors
        return RedirectToAction("Profile"); // Adjust as needed
    }

    [HttpGet]
    public IActionResult ResetPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await userManager.FindByEmailAsync(model.Email);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "User not found.");
            return View(model);
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var callbackUrl = Url.Action("UpdatePassword", "Auth",
            new CreateNewPasswordViewModel { Token = token, UserId = user.Id },
            Request.Scheme);

        await emailSender.SendEmailAsync(model.Email, "Reset Password",
            $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

        return RedirectToAction("ResetPasswordConfirmation");
    }

    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }

    [HttpGet]
    public IActionResult UpdatePassword(string token, string userId)
    {
        var model = new CreateNewPasswordViewModel { Token = token, UserId = userId };
        return View("CreateNewPassword", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePassword(CreateNewPasswordViewModel model)
    {
        if (!ModelState.IsValid) return View("CreateNewPassword", model);

        var user = await userManager.FindByIdAsync(model.UserId);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "User not found.");
            return BadRequest();
        }

        var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);

        if (result.Succeeded) return RedirectToAction("Login");

        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

        return View("CreateNewPassword", model);
    }
}