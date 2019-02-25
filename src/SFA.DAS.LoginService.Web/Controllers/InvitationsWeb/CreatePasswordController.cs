using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;

namespace SFA.DAS.LoginService.Web.Controllers.InvitationsWeb
{
    public class CreatePasswordController : Controller
    {
        [HttpGet("/Invitations/CreatePassword/{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            return View("CreatePassword", new CreatePasswordViewModel(){InvitationId = id, Password = "", ConfirmPassword = ""});
        }

        [HttpPost("/Invitations/CreatePassword/{id}")]
        public async Task<ActionResult> Post(CreatePasswordViewModel vm)
        {
            return Ok();
        }
    }
}