using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;

namespace SFA.DAS.LoginService.Application.ConfirmEmail
{
    public class CheckConfirmEmailHandler : IRequestHandler<CheckConfirmEmailRequest, CheckConfirmEmailResponse>
    {
        private readonly LoginContext _loginContext;

        public CheckConfirmEmailHandler(LoginContext loginContext)
        {
            _loginContext = loginContext;
        }

        public async Task<CheckConfirmEmailResponse> Handle(CheckConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            var checkEmailRequest = await _loginContext.ConfirmEmailRequests.SingleOrDefaultAsync(r => 
                r.Id == request.RequestId && 
                r.ValidUntil > SystemTime.UtcNow() &&
                r.IsComplete == false, cancellationToken);

            return checkEmailRequest == null 
                ? new CheckConfirmEmailResponse() 
                : new CheckConfirmEmailResponse
                {
                    IsValid = true, 
                    Email = checkEmailRequest.Email, 
                    RequestId = checkEmailRequest.Id
                };
        }
    }
}