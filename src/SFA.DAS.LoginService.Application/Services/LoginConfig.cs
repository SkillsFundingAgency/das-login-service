using SFA.DAS.LoginService.Application.Interfaces;

namespace SFA.DAS.LoginService.Application.Services
{
    public class LoginConfig : ILoginConfig
    {
        public string BaseUrl
        {
            get => "https://localhost:5001/";
            set{}
        }
    }
}