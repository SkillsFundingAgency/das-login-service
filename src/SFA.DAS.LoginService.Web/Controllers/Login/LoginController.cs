using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.BuildLoginViewModel;

namespace SFA.DAS.LoginService.Web.Controllers.Login
{
    public class LoginController : Controller
    {
        private readonly IMediator _mediator;

        public LoginController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/Account/Login")]
        public async Task<IActionResult> GetLogin(string redirectUrl)
        {
            var viewModel = await _mediator.Send(new BuildLoginViewModelRequest() {returnUrl = redirectUrl});
            return View("Login", viewModel);
        }
    }
}