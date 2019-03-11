using Microsoft.Extensions.Configuration;

namespace SFA.DAS.LoginService.Application.Interfaces
{
    public interface ILoginConfig
    {
        string BaseUrl { get; set; }
        string SqlConnectionString { get; set; }
        int PasswordResetExpiryInHours { get; set; }
        string CertificateThumbprint { get; set; }       
        NotificationsApiConfiguration NotificationsApiConfiguration { get; set; } 
    }
}