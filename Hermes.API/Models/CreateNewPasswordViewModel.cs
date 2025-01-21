using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Hermes.API.Models;

public class CreateNewPasswordViewModel
{
    [Required] public string Token { get; set; } = string.Empty;

    [Required] public string UserId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{6,}$", 
        ErrorMessage = "Password must include at least one uppercase letter, one lowercase letter, one number, and one special character.")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DisplayName("Confirm Password")]
    [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}