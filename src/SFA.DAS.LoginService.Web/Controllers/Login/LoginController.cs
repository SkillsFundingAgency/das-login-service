using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using SFA.DAS.LoginService.Application.BuildLoginViewModel;
using SFA.DAS.LoginService.Application.ProcessLogin;
using SFA.DAS.LoginService.Web.ClientComponents;

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
        public async Task<IActionResult> GetLogin(string returnUrl)
        {
            if(CreateAccountRedirect.GetCreateAccountRedirect(returnUrl) == true)
            {
                // clear the request to redirect to Create Account before redirecting
                returnUrl = CreateAccountRedirect.SetCreateAccountRedirect(returnUrl, false);
                return RedirectToAction("Get", "CreateAccount", new { returnUrl });
            }

            var viewModel = await _mediator.Send(new BuildLoginViewModelRequest() {returnUrl = returnUrl});
            return View("Login", viewModel);
        }

        [HttpPost("/Account/Login")]
        public async Task<IActionResult> PostLogin(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = await _mediator.Send(new BuildLoginViewModelRequest()
                    {returnUrl = loginViewModel.ReturnUrl});
                
                return View("Login", viewModel);
            }

            var loginResult = await _mediator.Send(new ProcessLoginRequest
            {
                Username = loginViewModel.Username, 
                Password = loginViewModel.Password,
                RememberLogin = loginViewModel.RememberLogin, 
                ReturnUrl = loginViewModel.ReturnUrl
            });

            if( loginResult.EmailConfirmationRequired)
            {
                return RedirectToAction("EmailConfirmationRequired", "ConfirmEmail", new { email = loginViewModel.Username, returnUrl = loginViewModel.ReturnUrl });
            }
            else if (loginResult.CredentialsValid)
            {
                return Redirect(loginViewModel.ReturnUrl);
            }

            ModelState.AddModelError("Username", loginResult.Message);

            var vm = await _mediator.Send(new BuildLoginViewModelRequest() {returnUrl = loginViewModel.ReturnUrl});
            vm.Password = loginViewModel.Password;
            vm.Username = loginViewModel.Username;
            
            return View("Login", vm);
        }
    }
}