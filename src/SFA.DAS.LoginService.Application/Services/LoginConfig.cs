using SFA.DAS.LoginService.Application.Interfaces;

namespace SFA.DAS.LoginService.Application.Services
{
    public class LoginConfig : ILoginConfig
    {
        //        public string BaseUrl
        //        {
        //            get => "https://localhost:5001/";
        //            set{}
        //        }
        //
        //        public string SqlConnectionString { 
        //            get => "Data Source=.\\sql;Initial Catalog=SFA.DAS.LoginService;Integrated Security=True";
        //            set { }
        //        }
        public string BaseUrl { get; set; }
        public string SqlConnectionString { get; set; }
    }
}