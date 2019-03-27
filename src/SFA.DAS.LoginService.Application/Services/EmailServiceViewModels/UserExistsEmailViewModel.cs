namespace SFA.DAS.LoginService.Application.Services.EmailServiceViewModels
{
    public class UserExistsEmailViewModel : EmailViewModel
    {
        public string Contact { get; set; }
        public string LoginLink { get; set; }
        public string ServiceName { get; set; }
        public string ServiceTeam { get; set; }
    }
}