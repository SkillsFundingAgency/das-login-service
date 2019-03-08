using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.GetClientById;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;

namespace SFA.DAS.LoginService.Web.Controllers.ResetPassword
{
    public class ResetPasswordController : Controller
    {
        private readonly IMediator _mediator;

        public ResetPasswordController(IMediator mediator)
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

        [HttpPost("/NewPassword/{clientId}/{requestId}")]
        public async Task<IActionResult> Post(Guid clientId, Guid requestId, ResetPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ResetPassword", viewModel);
            }

            if (viewModel.PasswordViewModel.Password != viewModel.PasswordViewModel.ConfirmPassword)
            {
                ModelState.AddModelError("PasswordViewModel.Password", "Passwords should match");
                return View("ResetPassword",
                    new ResetPasswordViewModel()
                    {
                        ClientId = clientId, Email = viewModel.Email, RequestId = requestId,
                        PasswordViewModel = new PasswordViewModel() {ConfirmPassword = viewModel.PasswordViewModel.ConfirmPassword, Password = viewModel.PasswordViewModel.Password}
                    });
            }

            var resetPasswordResponse = await _mediator.Send(new ResetUserPasswordRequest() {ClientId = clientId, Password = viewModel.PasswordViewModel.Password, RequestId = requestId});

            if (!resetPasswordResponse.IsSuccessful)
            {
                ModelState.AddModelError("PasswordViewModel.Password", "Password does not meet minimum complexity requirements");
                return View("ResetPassword",
                    new ResetPasswordViewModel()
                    {
                        ClientId = clientId, Email = viewModel.Email, RequestId = requestId,
                        PasswordViewModel = new PasswordViewModel() {ConfirmPassword = viewModel.PasswordViewModel.ConfirmPassword, Password = viewModel.PasswordViewModel.Password}
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