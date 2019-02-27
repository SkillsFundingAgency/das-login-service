using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.GetInvitationById
{
    public class GetInvitationByIdHandler : IRequestHandler<GetInvitationByIdRequest, Invitation>
    {
        private readonly LoginContext _loginContext;

        public GetInvitationByIdHandler(LoginContext loginContext)
        {
            _loginContext = loginContext;
        }
        
        public async Task<Invitation> Handle(GetInvitationByIdRequest request, CancellationToken cancellationToken)
        {
            var invitation = await _loginContext.Invitations.SingleOrDefaultAsync(i => i.Id == request.InvitationId);
            if (invitation == null)
            {
                return null;
            }
            
            return invitation.ValidUntil >= SystemTime.UtcNow() ? invitation : null;
        }
    }
}