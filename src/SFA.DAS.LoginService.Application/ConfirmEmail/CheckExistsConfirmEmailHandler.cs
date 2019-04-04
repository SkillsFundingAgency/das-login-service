using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;

namespace SFA.DAS.LoginService.Application.ConfirmEmail
{
    public class CheckExistsConfirmEmailHandler : IRequestHandler<CheckExistsConfirmEmailRequest, CheckExistsConfirmEmailResponse>
    {
        private readonly LoginContext _loginContext;

        public CheckExistsConfirmEmailHandler(LoginContext loginContext)
        {
            _loginContext = loginContext;
        }

        public async Task<CheckExistsConfirmEmailResponse> Handle(CheckExistsConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            var checkEmailRequest = await _loginContext.ConfirmEmailRequests
                .OrderByDescending(p => p.RequestedDate)
                .FirstOrDefaultAsync(r => r.Email == request.Email && r.IsComplete == false, cancellationToken);

            return checkEmailRequest == null
                ? new CheckExistsConfirmEmailResponse()
                : new CheckExistsConfirmEmailResponse
                {
                    HasRequest = true,
                    IsValid = checkEmailRequest.ValidUntil > SystemTime.UtcNow()
                };
        }
    }
}