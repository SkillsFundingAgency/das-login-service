using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels;

namespace SFA.DAS.LoginService.Web.Controllers.ResetPassword
{
    public class RequestPasswordResetController : BaseController
    {
        public RequestPasswordResetController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet("/ForgottenPassword/{clientId}")]
        public IActionResult Get(string clientId)
        {
            SetViewBagClientId(new Guid(clientId));

            var vm = new RequestPasswordResetViewModel(){ClientId = Guid.Parse(clientId)};
            return View("RequestPasswordReset", vm);            
        }

        [HttpPost("/ForgottenPassword/{clientId}")]
        public async Task<IActionResult> Post(Guid clientId, RequestPasswordResetViewModel requestPasswordResetViewModel)
        {
            SetViewBagClientId(clientId);

            if (!ModelState.IsValid)
            {
                return View("RequestPasswordReset", requestPasswordResetViewModel);
            }
            
            await Mediator.Send(new RequestPasswordResetRequest {ClientId = clientId, Email = requestPasswordResetViewModel.Email});
            return RedirectToAction("CodeSent", new { clientId, email=requestPasswordResetViewModel.Email});
        }

        [HttpGet("/CodeSent")]
        public IActionResult CodeSent(Guid clientId, string email)
        {
            SetViewBagClientId(clientId);

            return View("CodeSent", new CodeSentViewModel(){Email = email});
        }
    }
}