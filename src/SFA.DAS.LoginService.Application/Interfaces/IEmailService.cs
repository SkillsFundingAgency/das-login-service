using System.Threading.Tasks;

namespace SFA.DAS.LoginService.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendInvitationEmail(string email, string code, string signUpUrl);
    }
}