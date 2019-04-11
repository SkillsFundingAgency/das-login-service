using System;
using SFA.DAS.LoginService.Data;

namespace SFA.DAS.LoginService.Application.CreateAccount
{
    public class CreateAccountResponse
    {
        public CreateAccountResult CreateAccountResult { get; set; }
        public string Message { get; set; }
    }
}