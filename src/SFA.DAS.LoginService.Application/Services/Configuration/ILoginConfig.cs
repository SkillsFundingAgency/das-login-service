namespace SFA.DAS.LoginService.Application.Services.Configuration
{
    public interface ILoginConfig
    {
        string BaseUrl { get; set; }
        string SqlConnectionString { get; set; }
        int PasswordResetExpiryInHours { get; set; }
        string CertificateThumbprint { get; set; }
        NotificationsApiConfiguration NotificationsApiClientConfiguration { get; set; }
        int MaxFailedAccessAttempts { get; set; }
    }
}