using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Services;

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
            _logger.LogInformation($"SIGN UP Email sent to {viewModel.EmailAddress} with signupUrl {viewModel.LoginLink}");
        }

        public async Task SendResetPassword(PasswordResetEmailViewModel viewModel)
        {
            _logger.LogInformation($"FORGOT PASSWORD Email sent to {viewModel.EmailAddress} with resetPasswordUrl {viewModel.LoginLink}");
        }

        public async Task SendResetNoAccountPassword(PasswordResetNoAccountEmailViewModel viewModel)
        {
            _logger.LogInformation($"FORGOT PASSWORD BUT NO ACCOUNT Email sent to {viewModel.EmailAddress} with returnUrl {viewModel.LoginLink}");
        }
    }
}