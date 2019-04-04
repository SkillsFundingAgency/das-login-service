using System;

namespace SFA.DAS.LoginService.Web.Controllers.ConfirmEmail.ViewModels
{
    public class ConfirmEmailLinkViewModel
    {
        public string Email { get; set; }
        public string ReturnUrl { get; set; }
    }
}