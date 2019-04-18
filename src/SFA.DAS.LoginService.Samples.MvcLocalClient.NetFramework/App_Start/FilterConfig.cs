using System.Web;
using System.Web.Mvc;

namespace SFA.DAS.LoginService.Samples.MvcLocalClient.NetFramework
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
