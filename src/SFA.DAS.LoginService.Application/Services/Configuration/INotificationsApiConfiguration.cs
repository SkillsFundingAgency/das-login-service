namespace SFA.DAS.LoginService.Application.Services.Configuration
{
    public interface INotificationsApiConfiguration
    {
        string ApiBaseUrl { get; set; }
        string ClientToken { get; set; }
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string IdentifierUri { get; set; }
        string Tenant { get; set; }
    }
}