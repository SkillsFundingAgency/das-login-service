using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels;

namespace SFA.DAS.LoginService.Web.Controllers.ResetPassword
{
    public class ConfirmResetCodeController : Controller
    {
        private IMediator _mediator;

        public ConfirmResetCodeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet("/NewPassword/ConfirmCode/{clientId}/{requestId}")]
        public async Task<IActionResult> ConfirmCode(Guid clientId, Guid requestId)
        {
            var checkRequestResponse = await _mediator.Send(new CheckPasswordResetRequest {RequestId = requestId});
            return checkRequestResponse.IsValid
                ? View("ConfirmCode", new ConfirmResetCodeViewModel() {RequestId = requestId, Code = "", ClientId = clientId})
                : View("ExpiredLink", new ExpiredLinkViewModel() {ClientId = clientId});
        }
        
        [HttpPost("/NewPassword/ConfirmCode/{clientId}/{requestId}")]
        public async Task<IActionResult> ConfirmCode(ConfirmResetCodeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("ConfirmCode", vm);
            }

            var confirmResetCodeResponse = await _mediator.Send(new ConfirmResetCodeRequest(vm.RequestId, vm.Code));
            if (confirmResetCodeResponse.IsValid)
            {
                return RedirectToAction("Get", "ResetPassword", new {clientId = vm.ClientId, requestId = vm.RequestId});
            }

            ModelState.AddModelError("Code", "Code not valid");

            return View("ConfirmCode", vm);
        }
    }
}