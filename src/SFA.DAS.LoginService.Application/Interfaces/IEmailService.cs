using System.Threading.Tasks;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Application.Services.EmailServiceViewModels;

namespace SFA.DAS.LoginService.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendInvitationEmail(InvitationEmailViewModel viewModel);
        Task SendResetPassword(ResetPasswordEmailViewModel viewModel);
        Task SendResetNoAccountPassword(PasswordResetNoAccountEmailViewModel viewModel);
        Task SendPasswordReset(PasswordResetEmailViewModel viewModel);
        Task SendUserExistsEmail(UserExistsEmailViewModel viewModel);
        Task SendEmailConfirmation(EmailConfirmationEmailViewModel viewModel);
    }
}