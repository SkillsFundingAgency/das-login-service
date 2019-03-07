using System;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels
{
    public class CreatePasswordViewModel
    {
        public Guid InvitationId { get; set; }
        [Required(ErrorMessage = "Password validation message here")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password validation message here")]
        public string ConfirmPassword { get; set; }
    }
}