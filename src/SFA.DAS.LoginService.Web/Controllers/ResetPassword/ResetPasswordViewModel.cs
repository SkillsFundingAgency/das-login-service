using System;

namespace SFA.DAS.LoginService.Web.Controllers.ResetPassword
{
    public class ResetPasswordViewModel
    {
        public Guid ClientId { get; set; }
        public string Email { get; set; }
    }
}