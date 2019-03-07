using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.ResetPassword;

namespace SFA.DAS.LoginService.Web.Controllers.ResetPassword
{
    public class ResetPasswordController : Controller
    {
        private readonly IMediator _mediator;

        public ResetPasswordController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/ForgottenPassword/{clientId}")]
        public async Task<IActionResult> Get(string clientId)
        {
            var vm = new ResetPasswordViewModel(){ClientId = Guid.Parse(clientId)};
            return View("ResetPassword", vm);
        }

        [HttpPost("/ForgottenPassword/{clientId}")]
        public async Task<IActionResult> Post(Guid clientId, ResetPasswordViewModel resetPasswordViewModel)
        {
            await _mediator.Send(new ResetPasswordCodeRequest {ClientId = clientId, Email = resetPasswordViewModel.Email});
            return Ok();
        }
    }
}