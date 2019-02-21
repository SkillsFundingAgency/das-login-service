using System;
using System.Threading.Tasks;
using SFA.DAS.LoginService.Application.Interfaces;

namespace SFA.DAS.LoginService.Application.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendInvitationEmail(string email, string code, string signUpUrl)
        {
            Console.Write($"Email sent to {email} with code {code} and signupUrl {signUpUrl}");
        }
    }
}