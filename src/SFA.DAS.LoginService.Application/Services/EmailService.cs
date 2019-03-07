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
        
        public async Task SendInvitationEmail(string email, string code, string signUpUrl)
        {
            _logger.LogInformation($"SIGN UP Email sent to {email} with code {code} and signupUrl {signUpUrl}");
        }

        public async Task SendResetPassword(string email, string code, string resetPasswordUrl)
        {
            _logger.LogInformation($"FORGOT PASSWORD Email sent to {email} with code {code} and resetPasswordUrl {resetPasswordUrl}");
        }
    }
}