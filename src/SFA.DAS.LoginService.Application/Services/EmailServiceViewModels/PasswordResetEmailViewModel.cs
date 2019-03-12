namespace SFA.DAS.LoginService.Application.Services.EmailServiceViewModels
{
    public class PasswordResetEmailViewModel : EmailViewModel
    {
        public string Contact { get; set; }
        public string ServiceName { get; set; }
        public string ServiceTeam { get; set; }
        public string LoginLink { get; set; }
    }
}