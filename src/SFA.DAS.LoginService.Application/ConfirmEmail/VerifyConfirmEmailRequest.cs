using System;
using MediatR;

namespace SFA.DAS.LoginService.Application.ConfirmEmail
{
    public class VerifyConfirmEmailRequest : IRequest<VerifyConfirmEmailResponse>
    {
        public string IdentityToken { get; set; }
    }
}