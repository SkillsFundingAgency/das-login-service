namespace SFA.DAS.LoginService.Application.Services.EmailServiceViewModels
{
    public class PasswordResetNoAccountEmailViewModel : EmailViewModel
    {
        public string ServiceName { get; set; }
        public string ServiceTeam { get; set; }
        public string LoginLink { get; set; }
    }
}