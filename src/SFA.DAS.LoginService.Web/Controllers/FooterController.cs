using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.LoginService.Web.Controllers
{
    public class FooterController : Controller
    {
        [HttpGet("/TermsOfUse")]
        public IActionResult TermsOfUse()
        {
            return View();
        }
        [HttpGet("/Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet("/Cookies")]
        public IActionResult Cookies()
        {
            return View();
        }
    }
}