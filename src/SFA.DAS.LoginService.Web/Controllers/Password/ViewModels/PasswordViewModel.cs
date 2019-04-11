using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.LoginService.Web.Controllers.Password.ViewModels
{
    public class PasswordViewModel
    {
        public string Username { get; set; }

        [Required(ErrorMessage = "Enter a password")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Passwords should match")]
        public string ConfirmPassword { get; set; }
    }
}