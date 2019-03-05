using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.BuildLogoutViewModel;

namespace SFA.DAS.LoginService.Web.Controllers.Logout
{
    [AllowAnonymous]
    public class LogoutController : Controller
    {
        private readonly IMediator _mediator;

        public LogoutController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Account/Logout")]
        public async Task<IActionResult> Get(string logoutId)
        {
            var vm = await _mediator.Send(new LogoutRequest {LogoutId = logoutId});
            return View("Loggedout", vm);
        }
    }
}