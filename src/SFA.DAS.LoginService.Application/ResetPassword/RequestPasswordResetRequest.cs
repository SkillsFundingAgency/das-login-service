using System;
using MediatR;

namespace SFA.DAS.LoginService.Application.ResetPassword
{
    public class RequestPasswordResetRequest : IRequest
    {
        public string Email { get; set; }
        public Guid ClientId { get; set; }
    }
}