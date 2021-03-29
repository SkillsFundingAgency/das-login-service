namespace SFA.DAS.LoginService.Configuration
{
    public interface ILoginConfig
    {
        string BaseUrl { get; set; }
        string SqlConnectionString { get; set; }
        int PasswordResetExpiryInHours { get; set; }
        string CertificateThumbprint { get; set; }
        NotificationsApiConfiguration NotificationsApiClientConfiguration { get; set; }
        int MaxFailedAccessAttempts { get; set; }
        string DefaultSessionRedisConnectionString { get; set; }
        string DataProtectionKeysDatabase { get; set; }
    }
}