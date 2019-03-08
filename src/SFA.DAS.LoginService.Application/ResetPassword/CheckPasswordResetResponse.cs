using System;

namespace SFA.DAS.LoginService.Application.ResetPassword
{
    public class CheckPasswordResetResponse
    {
        public bool IsValid { get; set; }
        public Guid RequestId { get; set; }
        public string Email { get; set; }
    }
}