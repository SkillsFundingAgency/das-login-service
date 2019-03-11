using System.Threading.Tasks;

namespace SFA.DAS.LoginService.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendInvitationEmail(string email, string code, string signUpUrl);
        Task SendResetPassword(string email, string code, string resetPasswordUrl);
        Task SendResetNoAccountPassword(string email, string returnUrl);
    }
}