using System;

namespace SFA.DAS.LoginService.Application.ConfirmEmail
{
    public class CheckExistsConfirmEmailResponse
    {
        public bool HasRequest { get; set; }
        public bool IsValid { get; set; }
    }
}