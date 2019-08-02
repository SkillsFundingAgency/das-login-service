using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Application.BuildLoginViewModel;
using SFA.DAS.LoginService.Application.ProcessLogin;
using SFA.DAS.LoginService.Configuration;

namespace SFA.DAS.LoginService.Web.Controllers.Login
{
    public class LoginController : BaseController
    {
        public LoginController(IMediator mediator)
             : base(mediator)
        {
        }

        [HttpGet("/Account/Login")]
        public async Task<IActionResult> GetLogin(string returnUrl)
        {
            await SetViewBagClientIdByReturnUrl(returnUrl);
            
            var viewModel = await Mediator.Send(new BuildLoginViewModelRequest() {returnUrl = returnUrl});
            return View("Login", viewModel);
        }

        [HttpPost("/Account/Login")]
        public async Task<IActionResult> PostLogin(LoginViewModel loginViewModel)
        {
            await SetViewBagClientIdByReturnUrl(loginViewModel.ReturnUrl);
            
            if (!ModelState.IsValid)
            {
                var viewModel = await Mediator.Send(new BuildLoginViewModelRequest()
                    {returnUrl = loginViewModel.ReturnUrl});
                
                return View("Login", viewModel);
            }

            var loginResult = await Mediator.Send(new ProcessLoginRequest
            {
                Username = loginViewModel.Username, 
                Password = loginViewModel.Password,
                RememberLogin = loginViewModel.RememberLogin, 
                ReturnUrl = loginViewModel.ReturnUrl
            });

            if (loginResult.CredentialsValid)
            {
                return Redirect(loginViewModel.ReturnUrl);
            }

            ModelState.AddModelError("Username", loginResult.Message);
            ModelState.AddModelError("Password", "");

            var vm = await Mediator.Send(new BuildLoginViewModelRequest() {returnUrl = loginViewModel.ReturnUrl});
            vm.Password = loginViewModel.Password;
            vm.Username = loginViewModel.Username;
            
            return View("Login", vm);
        }
    }
}