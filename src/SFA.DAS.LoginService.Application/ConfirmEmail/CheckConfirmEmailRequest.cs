using System;
using MediatR;

namespace SFA.DAS.LoginService.Application.ConfirmEmail
{
    public class CheckConfirmEmailRequest : IRequest<CheckConfirmEmailResponse>
    {
        public Guid RequestId { get; set; }
    }
}