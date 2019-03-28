using System;

namespace SFA.DAS.LoginService.Application.ConfirmEmail
{
    public class CheckConfirmEmailResponse
    {
        public bool IsValid { get; set; }
        public Guid RequestId { get; set; }
        public string Email { get; set; }
    }
}