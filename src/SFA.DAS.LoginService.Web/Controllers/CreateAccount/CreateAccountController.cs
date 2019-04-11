using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.LoginService.Web.Controllers.CreateAccount.ViewModels;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Application.CreateAccount;

namespace SFA.DAS.LoginService.Web.Controllers.CreateAccountController
{
    public class CreateAccountController : Controller
    {
        private readonly IMediator _mediator;

        public CreateAccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/CreateAccount/{returnUrl}")]
        public IActionResult Get(string returnUrl)
        {
            return View("CreateAccount", new CreateAccountViewModel()
            {
                ReturnUrl = Uri.UnescapeDataString(returnUrl)
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateAccountViewModel createAccountViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateAccount", createAccountViewModel);
            }

            var createAccountResponse = await _mediator.Send(new CreateAccountRequest()
            {
                GivenName = createAccountViewModel.GivenName,
                FamilyName = createAccountViewModel.FamilyName,
                Username = createAccountViewModel.Username,
                Password = createAccountViewModel.Password
            });

            if (createAccountResponse.CreateAccountResult == CreateAccountResult.PasswordComplexityInvalid)
            {
                return PostErrorHelper(createAccountViewModel, "Password", createAccountResponse.Message);
            }
            else if (createAccountResponse.CreateAccountResult == CreateAccountResult.UsernameAlreadyTaken)
            {
                return PostErrorHelper(createAccountViewModel, "Username", createAccountResponse.Message);
            }
            else if (createAccountResponse.CreateAccountResult == CreateAccountResult.UnableToCreateAccount)
            {
                return PostErrorHelper(createAccountViewModel, "Username", createAccountResponse.Message);
            }

            return RedirectToAction("Get", "RequestConfirmEmail", new { returnUrl = createAccountViewModel.ReturnUrl, email = createAccountViewModel.Username });
        }

        public IActionResult PostErrorHelper(CreateAccountViewModel viewModel, string modelKey, string modelErrorMessage)
        {
            ModelState.AddModelError(modelKey, modelErrorMessage);
            return View("CreateAccount",
                new CreateAccountViewModel()
                {
                    GivenName = viewModel.GivenName,
                    FamilyName = viewModel.FamilyName,
                    Username = viewModel.Username,
                    ReturnUrl = viewModel.ReturnUrl,
                    Password = viewModel.Password,
                    ConfirmPassword = viewModel.ConfirmPassword
                });
        }
    }
}