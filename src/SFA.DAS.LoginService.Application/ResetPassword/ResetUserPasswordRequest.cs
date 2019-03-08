using System;
using MediatR;

namespace SFA.DAS.LoginService.Application.ResetPassword
{
    public class ResetUserPasswordRequest : IRequest<ResetPasswordResponse>
    {
        public Guid ClientId { get; set; }
        public Guid RequestId { get; set; }
        public string Password { get; set; }
    }
}