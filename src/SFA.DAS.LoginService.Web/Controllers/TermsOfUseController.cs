using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.LoginService.Web.Controllers
{
    public class Terms : Controller
    {
        public IActionResult TermsOfUse()
        {
            return View();
        }
    }
}