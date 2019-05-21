using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Types.GetClientById;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels;

namespace SFA.DAS.LoginService.Web.Controllers.ResetPassword
{
    public class ResetPasswordController : BaseController
    {
        public ResetPasswordController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpGet("/NewPassword/{clientId}/{requestId}")]
        public async Task<IActionResult> Get(Guid clientId, Guid requestId)
        {
            SetViewBagClientId(clientId);

            var checkRequestResponse = await Mediator.Send(new CheckPasswordResetRequest {RequestId = requestId});
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
            SetViewBagClientId(clientId);

            if (!ModelState.IsValid)
            {
                return View("ResetPassword", viewModel);
            }

            if (viewModel.Password != viewModel.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords should match");
                return View("ResetPassword",
                    new ResetPasswordViewModel()
                    {
                        ClientId = clientId, RequestId = requestId,
                        Password = viewModel.Password,
                        ConfirmPassword = viewModel.ConfirmPassword
                    });
            }

            var resetPasswordResponse = await Mediator.Send(new ResetUserPasswordRequest() {ClientId = clientId, Password = viewModel.Password, RequestId = requestId});

            if (!resetPasswordResponse.IsSuccessful)
            {
                ModelState.AddModelError("Password", "Password does not meet minimum complexity requirements");
                return View("ResetPassword",
                    new ResetPasswordViewModel()
                    {
                        ClientId = clientId, RequestId = requestId,
                        Password = viewModel.Password,
                        ConfirmPassword = viewModel.ConfirmPassword
                    });
            }

            return RedirectToAction("PasswordResetSuccessful", new {clientId = clientId});
        }

        public async Task<IActionResult> PasswordResetSuccessful(Guid clientId)
        {
            SetViewBagClientId(clientId);

            var client = await Mediator.Send(new GetClientByIdRequest() {ClientId = clientId });

            return View("PasswordResetSuccessful", new PasswordResetSuccessfulViewModel() {ReturnUrl = client.ServiceDetails.PostPasswordResetReturnUrl, ServiceName = client.ServiceDetails.ServiceName});
        }
    }
}