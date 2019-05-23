using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Web.Controllers.ViewModels;

namespace SFA.DAS.LoginService.Web.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet("/Error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
        }
    }
}