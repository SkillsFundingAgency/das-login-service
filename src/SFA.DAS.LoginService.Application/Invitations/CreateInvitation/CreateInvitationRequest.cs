using System;
using MediatR;

namespace SFA.DAS.LoginService.Application.Invitations.CreateInvitation
{
    public class CreateInvitationRequest : IRequest<CreateInvitationResponse>
    {
        public string Email { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string SourceId { get; set; }
        public Uri UserRedirect { get; set; }
        public Uri Callback { get; set; }
    }
}