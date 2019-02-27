using System;

namespace SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels
{
    public class SignUpCompleteViewModel
    {
        public Uri UserRedirectUri { get; set; }
        public string ServiceName { get; set; }
    }
}