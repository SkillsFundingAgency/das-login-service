using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.GetClientById;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Web.Controllers.Password.ViewModels;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels;

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
                    Username = checkRequestResponse.Email,
                    Password = "",
                    ConfirmPassword = "", 
                    RequestId = requestId, 
                    ClientId = clientId}) 
                : View("ExpiredLink", new ExpiredLinkViewModel(){ClientId = clientId});
        }

        [HttpPost("/NewPassword/{clientId}/{requestId}")]
        public async Task<IActionResult> Post(Guid clientId, Guid requestId, ResetPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ResetPassword", viewModel);
            }

            var resetPasswordResponse = await _mediator.Send(new ResetUserPasswordRequest() {ClientId = clientId, Password = viewModel.Password, RequestId = requestId});

            if (!resetPasswordResponse.IsSuccessful)
            {
                ModelState.AddModelError("Password", "Password does not meet minimum complexity requirements");
                return View("ResetPassword",
                    new ResetPasswordViewModel()
                    {
                        ClientId = clientId,
                        RequestId = requestId,
                        Password = viewModel.Password,
                        ConfirmPassword = viewModel.ConfirmPassword
                    });
            }

            return RedirectToAction("PasswordResetSuccessful", new {clientId = clientId});
        }

        public async Task<IActionResult> PasswordResetSuccessful(Guid clientid)
        {
            var client = await _mediator.Send(new GetClientByIdRequest() {ClientId = clientid});

            return View("PasswordResetSuccessful", new PasswordResetSuccessfulViewModel() {ReturnUrl = client.ServiceDetails.PostPasswordResetReturnUrl, ServiceName = client.ServiceDetails.ServiceName});
        }

       
    }
}