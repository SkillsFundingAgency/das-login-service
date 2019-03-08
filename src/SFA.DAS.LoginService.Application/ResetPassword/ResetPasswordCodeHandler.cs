using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.ResetPassword
{
    public class ResetPasswordCodeHandler : IRequestHandler<ResetPasswordCodeRequest>
    {
        private readonly IEmailService _emailService;
        private readonly ICodeGenerationService _codeGenerationService;
        private readonly ILoginConfig _loginConfig;
        private readonly LoginContext _loginContext;
        private readonly IUserService _userService;
        private readonly IHashingService _hashingService;

        public ResetPasswordCodeHandler(IEmailService emailService, ICodeGenerationService codeGenerationService,
            ILoginConfig loginConfig, LoginContext loginContext, IUserService userService,
            IHashingService hashingService)
        {
            _emailService = emailService;
            _codeGenerationService = codeGenerationService;
            _loginConfig = loginConfig;
            _loginContext = loginContext;
            _userService = userService;
            _hashingService = hashingService;
        }

        public async Task<Unit> Handle(ResetPasswordCodeRequest request, CancellationToken cancellationToken)
        {
            var loginUser = await _userService.FindByEmail(request.Email);
            if (loginUser == null)
            {
                return Unit.Value;
            }

            await ClearOutAnyPreviousStillValidRequests(request.Email);
            
            await _userService.LockoutUser(request.Email);
            
            var plainTextCode = _codeGenerationService.GenerateCode();

            var resetPasswordRequest = await SavePasswordRequest(request, cancellationToken, plainTextCode);

            var resetUri = new Uri(new Uri(_loginConfig.BaseUrl), $"NewPassword/{request.ClientId}/{resetPasswordRequest.Id}");
            
            await _emailService.SendResetPassword(request.Email, plainTextCode, resetUri.ToString());
            return Unit.Value;
        }

        private async Task ClearOutAnyPreviousStillValidRequests(string email)
        {
            var stillValidRequests = await _loginContext.ResetPasswordRequests.Where(r => r.ValidUntil > SystemTime.UtcNow() && r.IsComplete == false).ToListAsync();
            stillValidRequests.ForEach(r => r.ValidUntil = SystemTime.UtcNow().AddDays(-1));
            await _loginContext.SaveChangesAsync();
        }

        private async Task<ResetPasswordRequest> SavePasswordRequest(ResetPasswordCodeRequest request, CancellationToken cancellationToken,
            string plainTextCode)
        {
            var resetPasswordRequest = new ResetPasswordRequest()
            {
                ClientId = request.ClientId,
                Code = _hashingService.GetHash(plainTextCode),
                IsComplete = false,
                ValidUntil = SystemTime.UtcNow().AddHours(_loginConfig.PasswordResetExpiryInHours),
                Email = request.Email,
                RequestedDate = SystemTime.UtcNow()
            };
            _loginContext.ResetPasswordRequests.Add(resetPasswordRequest);
            await _loginContext.SaveChangesAsync(cancellationToken);
            return resetPasswordRequest;
        }
    }
}