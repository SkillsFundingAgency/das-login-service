using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels
{
    public class PasswordViewModel
    {
        public string Username { get; set; }
        [Required(ErrorMessage = "Enter a password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm your password")]
        public string ConfirmPassword { get; set; }
    }
}