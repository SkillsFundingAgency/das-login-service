using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.BuildLogoutViewModel;
using SFA.DAS.LoginService.Configuration;

namespace SFA.DAS.LoginService.Web.Controllers.Logout
{
    [AllowAnonymous]
    public class LogoutController : BaseController
    {
        public LogoutController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet("Account/Logout")]
        public async Task<IActionResult> Get(string logoutId)
        {
            await SetViewBagClientIdByLogoutId(logoutId);

            var vm = await Mediator.Send(new LogoutRequest {LogoutId = logoutId});
            return View("Loggedout", vm);
        }
    }
}