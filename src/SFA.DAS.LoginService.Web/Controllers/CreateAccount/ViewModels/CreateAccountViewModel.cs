using System.ComponentModel.DataAnnotations;
using SFA.DAS.LoginService.Web.Controllers.Password.ViewModels;

namespace SFA.DAS.LoginService.Web.Controllers.CreateAccount.ViewModels
{
    public class CreateAccountViewModel : PasswordViewModel
    {
        [Required(ErrorMessage = "Enter a first name")]
        public string GivenName { get; set; }

        [Required(ErrorMessage = "Enter a last name")]
        public string FamilyName { get; set; }

        [Required(ErrorMessage = "Enter a email address")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public new string Username { get; set; }

        public string ReturnUrl { get; set; }
    }
}