using System;

namespace SFA.DAS.LoginService.Web.Controllers.ConfirmEmail.ViewModels
{
    public class ConfirmEmailExpiredLinkViewModel
    {
        public Guid ClientId { get; set; }
        public string Email { get; set; }
    }
}