using System.ComponentModel.DataAnnotations;

public class ResetPasswordViewModel
{
    [Required] [Display(Name = "Email")] public string Email { get; set; } = string.Empty;
}