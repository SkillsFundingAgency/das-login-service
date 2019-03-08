using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;

namespace SFA.DAS.LoginService.Application.ResetPassword
{
    public class CheckPasswordResetHandler : IRequestHandler<CheckPasswordResetRequest, CheckPasswordResetResponse>
    {
        private readonly LoginContext _loginContext;

        public CheckPasswordResetHandler(LoginContext loginContext)
        {
            _loginContext = loginContext;
        }

        public async Task<CheckPasswordResetResponse> Handle(CheckPasswordResetRequest request, CancellationToken cancellationToken)
        {
            var resetRequest = await _loginContext.ResetPasswordRequests.SingleOrDefaultAsync(r => 
                r.Id == request.RequestId && 
                r.ValidUntil > SystemTime.UtcNow() &&
                r.IsComplete == false, cancellationToken);

            return resetRequest == null 
                ? new CheckPasswordResetResponse() 
                : new CheckPasswordResetResponse
                {
                    IsValid = true, 
                    Email = resetRequest.Email, 
                    RequestId = resetRequest.Id
                };
        }
    }
}