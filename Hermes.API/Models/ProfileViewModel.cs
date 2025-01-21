using System.ComponentModel.DataAnnotations;

namespace Hermes.API.Models;
public class ProfileViewModel
{
    [Required]
    [MinLength(4)]
    [RegularExpression(@"^[a-zA-Z0-9._]+$",
    ErrorMessage = "Username can only contain letters, numbers, periods, and underscores.")]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
