using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;

namespace SFA.DAS.LoginService.Application.GetInvitationById
{
    public class GetInvitationByIdHandler : IRequestHandler<GetInvitationByIdRequest, InvitationResponse>
    {
        private readonly LoginContext _loginContext;

        public GetInvitationByIdHandler(LoginContext loginContext)
        {
            _loginContext = loginContext;
        }
        
        public async Task<InvitationResponse> Handle(GetInvitationByIdRequest request, CancellationToken cancellationToken)
        {
            var invitation = await _loginContext.Invitations.SingleOrDefaultAsync(i => i.Id == request.InvitationId);
            if (invitation == null)
            {
                return null;
            }
            
            return invitation.ValidUntil >= SystemTime.UtcNow() ? new InvitationResponse(invitation) : null;
        }
    }
}