using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;

namespace SFA.DAS.LoginService.Application.ResetPassword
{
    public class ResetPasswordHandler : IRequestHandler<ResetUserPasswordRequest, ResetPasswordResponse>
    {
        private readonly IUserService _userService;
        private readonly LoginContext _loginContext;

        public ResetPasswordHandler(IUserService userService, LoginContext loginContext)
        {
            _userService = userService;
            _loginContext = loginContext;
        }

        public async Task<ResetPasswordResponse> Handle(ResetUserPasswordRequest request, CancellationToken cancellationToken)
        {
            var resetRequest = await _loginContext.ResetPasswordRequests.SingleOrDefaultAsync(r => r.Id == request.RequestId && r.IsComplete == false && r.ValidUntil > SystemTime.UtcNow(), cancellationToken: cancellationToken);
            var userResponse = await _userService.ResetPassword(resetRequest.Email, request.Password, resetRequest.IdentityToken);

            if (userResponse.Result != IdentityResult.Success)
            {
                return new ResetPasswordResponse() {IsSuccessful = false};
            }

            var client = await _loginContext.Clients.SingleOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken);
            
            return new ResetPasswordResponse(){IsSuccessful = true, ClientId = request.ClientId, ReturnUrl = client.ServiceDetails.PostPasswordResetReturnUrl};
        }
    }
}