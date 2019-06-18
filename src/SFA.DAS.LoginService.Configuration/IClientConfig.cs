using SFA.DAS.LoginService.Data.JsonObjects;

namespace SFA.DAS.LoginService.Configuration
{
    public interface IClientConfig
    {
        ServiceDetails ServiceDetails { get; set; }
        bool AllowInvitationSignUp { get; set; }
        bool AllowLocalSignUp { get; set; }
    }
}
