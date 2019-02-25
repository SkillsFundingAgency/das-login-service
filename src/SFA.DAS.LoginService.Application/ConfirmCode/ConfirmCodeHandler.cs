using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Data;

namespace SFA.DAS.LoginService.Application.ConfirmCode
{
    public class ConfirmCodeHandler : IRequestHandler<ConfirmCodeRequest, ConfirmCodeResponse>
    {
        private readonly LoginContext _loginContext;
        private readonly IHashingService _hashingService;

        public ConfirmCodeHandler(LoginContext loginContext, IHashingService hashingService)
        {
            _loginContext = loginContext;
            _hashingService = hashingService;
        }

        public async Task<ConfirmCodeResponse> Handle(ConfirmCodeRequest request, CancellationToken cancellationToken)
        {
            var invitation = await _loginContext.Invitations.SingleOrDefaultAsync(i => i.Id == request.InvitationId, cancellationToken: cancellationToken);
            if (invitation == null)
            {
                return new ConfirmCodeResponse {IsValid = false};
            }

            if (invitation.Code == _hashingService.GetHash(request.Code))
            {
                invitation.CodeConfirmed = true;
                await _loginContext.SaveChangesAsync(cancellationToken);
                return new ConfirmCodeResponse {IsValid = true};
            }
            return new ConfirmCodeResponse();
        }
    }

    
}