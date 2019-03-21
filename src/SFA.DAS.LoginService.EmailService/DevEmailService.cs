using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Application.Services.EmailServiceViewModels;

namespace SFA.DAS.LoginService.EmailService
{
    public class DevEmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public DevEmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }
        
        public async Task SendInvitationEmail(InvitationEmailViewModel viewModel)
        {
            await Task.Run(() => _logger.LogInformation($"SIGN UP Email sent to {viewModel.EmailAddress} with signupUrl {viewModel.LoginLink}"));
        }

        public async Task SendResetPassword(ResetPasswordEmailViewModel viewModel)
        {
            await Task.Run(() => _logger.LogInformation($"FORGOT PASSWORD Email sent to {viewModel.EmailAddress} with resetPasswordUrl {viewModel.LoginLink}"));
        }

        public async Task SendResetNoAccountPassword(PasswordResetNoAccountEmailViewModel viewModel)
        {
            await Task.Run(() => _logger.LogInformation($"FORGOT PASSWORD BUT NO ACCOUNT Email sent to {viewModel.EmailAddress} with returnUrl {viewModel.LoginLink}"));
        }

        public async Task SendPasswordReset(PasswordResetEmailViewModel vm)
        {
            await Task.Run(() => _logger.LogInformation($"PASSWORD RESET Email sent to {vm.EmailAddress} with returnUrl {vm.LoginLink}"));
        }
    }
}