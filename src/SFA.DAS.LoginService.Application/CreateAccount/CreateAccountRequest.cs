using System;
using MediatR;

namespace SFA.DAS.LoginService.Application.CreateAccount
{
    public class CreateAccountRequest : IRequest<CreateAccountResponse>
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}