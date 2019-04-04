using SFA.DAS.LoginService.Data;
using System;

namespace SFA.DAS.LoginService.Application.ConfirmEmail
{
    public class VerifyConfirmEmailResponse
    {
        public VerifyConfirmedEmailResult VerifyConfirmedEmailResult { get; set; }
        public string Email { get; set; }
    }
}