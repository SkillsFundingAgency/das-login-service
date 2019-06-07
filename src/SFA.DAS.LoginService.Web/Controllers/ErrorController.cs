using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Web.Controllers.ViewModels;

namespace SFA.DAS.LoginService.Web.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet("/Error")]
        public IActionResult Error()
        {
            // the following view does not specify the client service details as there is no known
            // way of determining which client was using the login service when the error occurred
            return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
        }
    }
}