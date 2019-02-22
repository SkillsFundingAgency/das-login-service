using System;
using MediatR;

namespace SFA.DAS.LoginService.Application.GetInvitationById
{
    public class GetInvitationByIdRequest : IRequest<InvitationResponse>
    {
        public Guid InvitationId { get; }

        public GetInvitationByIdRequest(Guid invitationId)
        {
            InvitationId = invitationId;
        }
    }
}