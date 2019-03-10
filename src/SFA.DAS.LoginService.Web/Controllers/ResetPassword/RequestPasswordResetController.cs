using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels;

namespace SFA.DAS.LoginService.Web.Controllers.ResetPassword
{
    public class RequestPasswordResetController : Controller
    {
        private readonly IMediator _mediator;

        public RequestPasswordResetController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/ForgottenPassword/{clientId}")]
        public IActionResult Get(string clientId)
        {
            var vm = new RequestPasswordResetViewModel(){ClientId = Guid.Parse(clientId)};
            return View("RequestPasswordReset", vm);            
        }

        [HttpPost("/ForgottenPassword/{clientId}")]
        public async Task<IActionResult> Post(Guid clientId, RequestPasswordResetViewModel requestPasswordResetViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("RequestPasswordReset", requestPasswordResetViewModel);
            }
            
            await _mediator.Send(new RequestPasswordResetRequest {ClientId = clientId, Email = requestPasswordResetViewModel.Email});
            return RedirectToAction("CodeSent", new {email=requestPasswordResetViewModel.Email});
        }

        [HttpGet("/CodeSent")]
        public IActionResult CodeSent(string email)
        {
            return View("CodeSent", new CodeSentViewModel(){Email = email});
        }
    }
}