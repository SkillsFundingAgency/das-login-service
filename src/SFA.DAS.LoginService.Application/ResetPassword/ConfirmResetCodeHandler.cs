using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Data;

namespace SFA.DAS.LoginService.Application.ResetPassword
{
    public class ConfirmResetCodeHandler : IRequestHandler<ConfirmResetCodeRequest, ConfirmResetCodeResponse>
    {
        private readonly LoginContext _loginContext;
        private readonly IHashingService _hashingService;

        public ConfirmResetCodeHandler(LoginContext loginContext, IHashingService hashingService)
        {
            _loginContext = loginContext;
            _hashingService = hashingService;
        }
        
        public async Task<ConfirmResetCodeResponse> Handle(ConfirmResetCodeRequest request, CancellationToken cancellationToken)
        {
            var resetRequest = await _loginContext.ResetPasswordRequests.SingleOrDefaultAsync(r => r.Id == request.RequestId);

            var hashedSuppliedCode = _hashingService.GetHash(request.Code);

            return new ConfirmResetCodeResponse() {IsValid = hashedSuppliedCode == resetRequest.Code};
        }
    }
}