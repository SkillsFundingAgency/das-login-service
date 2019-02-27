using System;
using MediatR;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.GetInvitationById
{
    public class GetInvitationByIdRequest : IRequest<Invitation>
    {
        public Guid InvitationId { get; }

        public GetInvitationByIdRequest(Guid invitationId)
        {
            InvitationId = invitationId;
        }
    }
}