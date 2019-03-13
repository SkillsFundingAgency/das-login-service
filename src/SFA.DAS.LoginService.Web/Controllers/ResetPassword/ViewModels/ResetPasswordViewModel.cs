using System;

namespace SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels
{
    public class ResetPasswordViewModel
    {
        public Guid RequestId { get; set; }
        
        public PasswordViewModel PasswordViewModel { get; set; }
        
        public Guid ClientId { get; set; }
    }
}