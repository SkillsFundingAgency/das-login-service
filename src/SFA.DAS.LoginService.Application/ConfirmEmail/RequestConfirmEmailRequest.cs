using System;
using MediatR;

namespace SFA.DAS.LoginService.Application.ConfirmEmail
{
    public class RequestConfirmEmailRequest : IRequest
    {
        public string Email { get; set; }
        public Guid ClientId { get; set; }
    }
}