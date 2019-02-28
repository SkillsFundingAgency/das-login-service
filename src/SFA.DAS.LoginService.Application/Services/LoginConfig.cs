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

        public string SqlConnectionString { 
            //get => "Data Source=.\\sql;Initial Catalog=SFA.DAS.LoginService;Integrated Security=True";
            get => "Server=tcp:esfatemp.database.windows.net,1433;Initial Catalog=SFA.DAS.LoginService;Persist Security Info=False;User ID=esfa;Password=C0ventry18;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            set { }
        }
    }
}