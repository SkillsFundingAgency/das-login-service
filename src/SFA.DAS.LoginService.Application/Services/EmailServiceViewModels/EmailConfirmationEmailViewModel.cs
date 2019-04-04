namespace SFA.DAS.LoginService.Application.Services.EmailServiceViewModels
{
    public class EmailConfirmationEmailViewModel : EmailViewModel
    {
        public string Contact { get; set; }
        public string ServiceName { get; set; }
        public string ServiceTeam { get; set; }
        public string ConfirmLink { get; set; }
    }
}