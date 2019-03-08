using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.LoginService.Web.Controllers.ResetPassword
{
    public class RequestPasswordResetViewModel
    {
        public Guid ClientId { get; set; }
        [Required(ErrorMessage = "Please enter an email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
    }
}