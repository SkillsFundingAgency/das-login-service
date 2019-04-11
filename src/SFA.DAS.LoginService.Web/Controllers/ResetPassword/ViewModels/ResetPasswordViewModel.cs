using System;
using SFA.DAS.LoginService.Web.Controllers.Password.ViewModels;

namespace SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels
{
    public class ResetPasswordViewModel : PasswordViewModel
    {
        public Guid RequestId { get; set; }
        
        public Guid ClientId { get; set; }
    }
}