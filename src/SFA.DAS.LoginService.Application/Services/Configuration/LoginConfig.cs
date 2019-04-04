
namespace SFA.DAS.LoginService.Application.Services.Configuration
{
    public class LoginConfig : ILoginConfig
    {
        public string BaseUrl { get; set; }
        public string SqlConnectionString { get; set; }
        public int PasswordResetExpiryInHours { get; set; }
        public int ConfirmEmailExpiryInHours { get; set; }
        public int MaxFailedAccessAttempts { get; set; }
        public string CertificateThumbprint { get; set; }
        public NotificationsApiConfiguration NotificationsApiClientConfiguration { get; set; }
    }
}