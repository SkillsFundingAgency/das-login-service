using System;
using MediatR;

namespace SFA.DAS.LoginService.Application.ResetPassword
{
    public class CheckPasswordResetRequest : IRequest<CheckPasswordResetResponse>
    {
        public Guid RequestId { get; set; }
    }
}