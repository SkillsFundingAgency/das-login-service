using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Application.Invitations.CreateInvitation;

namespace SFA.DAS.LoginService.Application.Reinvite
{
    public class ReinviteHandler : IRequestHandler<ReinviteRequest, CreateInvitationResponse>
    {
        private readonly IMediator _mediator;

        public ReinviteHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CreateInvitationResponse> Handle(ReinviteRequest request, CancellationToken cancellationToken)
        {
            var invitation = await _mediator.Send(new GetInvitationByIdRequest(request.InvitationId), CancellationToken.None);

            var response = await _mediator.Send(
                new CreateInvitationRequest
                {
                    Callback = invitation.CallbackUri, 
                    ClientId = invitation.ClientId, 
                    SourceId = invitation.SourceId,
                    Email = invitation.Email,
                    FamilyName = invitation.FamilyName,
                    GivenName = invitation.GivenName,
                    UserRedirect = invitation.UserRedirectUri,
                    Inviter = invitation.Inviter,
                    InviterId = invitation.InviterId              
                }, CancellationToken.None);

            return response;
        }
    }
}