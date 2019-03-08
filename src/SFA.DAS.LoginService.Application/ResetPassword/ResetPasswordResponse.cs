using System;

namespace SFA.DAS.LoginService.Application.ResetPassword
{
    public class ResetPasswordResponse
    {
        public bool IsSuccessful { get; set; }
        public Guid ClientId { get; set; }
        public string ReturnUrl { get; set; }
    }
}