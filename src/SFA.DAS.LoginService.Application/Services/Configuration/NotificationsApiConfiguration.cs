namespace SFA.DAS.LoginService.Application.Services.Configuration
{
    public class NotificationsApiConfiguration : INotificationsApiConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string ClientToken { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string IdentifierUri { get; set; }
        public string Tenant { get; set; }
    }  
}