using System;

namespace SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels
{
    public class CreatePasswordViewModel
    {
        public Guid InvitationId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}