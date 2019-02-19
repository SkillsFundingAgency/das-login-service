using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.LoginService.Web.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class InvitationsController : Controller
    {
        [HttpPost("/Invitations")]
        public IActionResult Invite()
        {
            return Ok();
        }
    }
}