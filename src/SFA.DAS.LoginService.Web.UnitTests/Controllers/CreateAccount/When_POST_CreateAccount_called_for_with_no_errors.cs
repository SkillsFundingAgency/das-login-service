using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.CreateAccount;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Web.Controllers.CreateAccount.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.CreateAccount
{
    public class When_POST_CreateAccount_called_for_with_no_errors : CreateAccountTestBase
    {
        [SetUp]
        public void Arrange()
        {
            Mediator.Send(Arg.Any<CreateAccountRequest>()).Returns(new CreateAccountResponse()
            {
                CreateAccountResult = CreateAccountResult.Successful
            });
        }

        [Test]
        public async Task Then_ViewResult_is_returned_with_ModelState_invalid()
        {
            var viewModel = new CreateAccountViewModel()
            {
                Username = "email+address@company.com",
                Password = "Pa55word",
                ConfirmPassword = "Pa55word",
                ReturnUrl = "http://returnhere.com"

            };

            var result = await Controller.Post(viewModel);

            result.Should().BeOfType<RedirectToActionResult>();
            ((RedirectToActionResult)result).ControllerName.Should().Be("RequestConfirmEmail");
            ((RedirectToActionResult)result).ActionName.Should().Be("Get");
            ((RedirectToActionResult)result).RouteValues.Should().Contain(new KeyValuePair<string, object>("returnUrl", viewModel.ReturnUrl));
            ((RedirectToActionResult)result).RouteValues.Should().Contain(new KeyValuePair<string, object>("email", viewModel.Username));
        }
    }
}