using SFA.DAS.LoginService.Application.Interfaces;

namespace SFA.DAS.LoginService.Application.Services
{
    public class LoginConfig : ILoginConfig
    {
        public string BaseUrl
        {
            get => "https://localhost:5001/";
            set { }
        }

        public string SqlConnectionString
        {
            get => "Server=tcp:esfatemp.database.windows.net,1433;Initial Catalog=SFA.DAS.LoginService;Persist Security Info=False;User ID=esfa;Password=qHtxWjvcqAz7A0R;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            set { }
        }
    }
}