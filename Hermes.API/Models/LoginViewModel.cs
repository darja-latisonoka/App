using System.ComponentModel.DataAnnotations;

namespace Hermes.API.Models;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Username")]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Required]
    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}