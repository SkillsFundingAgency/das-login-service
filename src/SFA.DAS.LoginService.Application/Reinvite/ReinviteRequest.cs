using System;
using MediatR;
using SFA.DAS.LoginService.Application.Invitations.CreateInvitation;

namespace SFA.DAS.LoginService.Application.Reinvite
{
    public class ReinviteRequest : IRequest<CreateInvitationResponse>
    {
        public Guid InvitationId { get; set; }
    }
}