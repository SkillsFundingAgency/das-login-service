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

        [HttpGet("/NewPassword/{clientId}/{requestId}")]
        public async Task<IActionResult> Get(Guid clientId, Guid requestId)
        {
            var checkRequestResponse = await _mediator.Send(new CheckPasswordResetRequest {RequestId = requestId});
            return checkRequestResponse.IsValid 
                ? View("ResetPassword", new ResetPasswordViewModel(){
                    PasswordViewModel = new PasswordViewModel(){Password = "", ConfirmPassword = ""},
                    Email = checkRequestResponse.Email, 
                    RequestId = requestId, 
                    ClientId = clientId}) 
                : View("ExpiredLink", new ExpiredLinkViewModel(){ClientId = clientId});
        }
    }
}