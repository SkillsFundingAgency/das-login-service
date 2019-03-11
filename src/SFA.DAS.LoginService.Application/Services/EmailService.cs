using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SFA.DAS.LoginService.Application.Interfaces;

namespace SFA.DAS.LoginService.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }
        
        public async Task SendInvitationEmail(InvitationEmailViewModel vm)
        {
            _logger.LogInformation($"SIGN UP Email sent to {vm.EmailAddress} with signupUrl {vm.LoginLink}");
        }

        public async Task SendResetPassword(string email, string resetPasswordUrl)
        {
            _logger.LogInformation($"FORGOT PASSWORD Email sent to {email} with resetPasswordUrl {resetPasswordUrl}");
        }

        public async Task SendResetNoAccountPassword(string email, string returnUrl)
        {
            _logger.LogInformation($"FORGOT PASSWORD BUT NO ACCOUNT Email sent to {email} with returnUrl {returnUrl}");
        }
    }
}