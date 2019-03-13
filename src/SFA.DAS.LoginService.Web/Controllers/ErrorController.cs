using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.LoginService.Web.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet("/Error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}