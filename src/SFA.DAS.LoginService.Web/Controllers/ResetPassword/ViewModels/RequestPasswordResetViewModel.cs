using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels
{
    public class RequestPasswordResetViewModel
    {
        public Guid ClientId { get; set; }
        [Required(ErrorMessage = "Enter an email address")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; }
    }
}