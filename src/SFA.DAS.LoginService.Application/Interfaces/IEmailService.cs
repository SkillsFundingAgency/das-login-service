using System.Threading.Tasks;
using SFA.DAS.LoginService.Application.Services;

namespace SFA.DAS.LoginService.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendInvitationEmail(InvitationEmailViewModel viewModel);
        Task SendResetPassword(string email, string resetPasswordUrl);
        Task SendResetNoAccountPassword(string email, string returnUrl);
    }
}