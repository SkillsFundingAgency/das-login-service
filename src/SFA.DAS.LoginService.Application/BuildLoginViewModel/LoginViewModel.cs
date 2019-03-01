namespace SFA.DAS.LoginService.Application.BuildLoginViewModel
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberLogin { get; set; }
        public string ReturnUrl { get; set; }
        public bool AllowRememberLogin { get; set; }
        public bool EnableLocalLogin { get; set; }
    }
}