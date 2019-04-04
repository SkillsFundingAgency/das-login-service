namespace SFA.DAS.LoginService.Application.ProcessLogin
{
    public class ProcessLoginResponse
    {
        public bool CredentialsValid { get; set; }
        public bool EmailConfirmationRequired { get; set; }
        public string Message { get; set; }
    }
}