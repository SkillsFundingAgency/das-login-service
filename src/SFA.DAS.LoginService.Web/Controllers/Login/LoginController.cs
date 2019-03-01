using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.LoginService.Web.Controllers.Login
{
    public class LoginController : Controller
    {
        [HttpGet("/Account/Login")]
        public IActionResult GetLogin()
        {
            return Ok();
        }
    }
}