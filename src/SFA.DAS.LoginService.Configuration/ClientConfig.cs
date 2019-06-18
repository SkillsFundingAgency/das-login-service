using SFA.DAS.LoginService.Data.JsonObjects;

namespace SFA.DAS.LoginService.Configuration
{
    public class ClientConfig : IClientConfig
    {
        public ServiceDetails ServiceDetails { get; set; }
        public bool AllowInvitationSignUp { get; set; }
        public bool AllowLocalSignUp { get; set; }
    }
}
