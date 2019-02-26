using System;
using MediatR;

namespace SFA.DAS.LoginService.Application.CreatePassword
{
    public class CreatePasswordRequest : IRequest<CreatePasswordResponse>
    {
        public Guid InvitationId { get; set; }
        public string Password { get; set; }
    }
}