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
            if (!ModelState.IsValid)
            {
                return View("ResetPassword", resetPasswordViewModel);
            }
            
            await _mediator.Send(new ResetPasswordCodeRequest {ClientId = clientId, Email = resetPasswordViewModel.Email});
            return RedirectToAction("CodeSent", new {email=resetPasswordViewModel.Email});
        }

        [HttpGet("/CodeSent")]
        public async Task<IActionResult> CodeSent(string email)
        {
            return View("CodeSent", new CodeSentViewModel(){Email = email});
        }
    }

    public class CodeSentViewModel
    {
        public string Email { get; set; }
    }
}