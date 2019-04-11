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
    public class When_POST_CreateAccount_called_for_username_which_exists : CreateAccountTestBase
    {
        private string _expectedError = "EXISTING USERNAME ERROR";

        [SetUp]
        public void Arrange()
        {
            Mediator.Send(Arg.Any<CreateAccountRequest>()).Returns(new CreateAccountResponse()
            {
                CreateAccountResult = CreateAccountResult.UsernameAlreadyTaken,
                Message = _expectedError
            });
        }

        [Test]
        public async Task Then_ViewResult_is_returned_with_ModelState_invalid()
        {
            var result = await Controller.Post(new CreateAccountViewModel()
            {
                Username = "email+address@company.com",
                Password = "Pa55word",
                ConfirmPassword = "Pa55word"
            });

            result.Should().BeOfType<ViewResult>();
            ((ViewResult)result).ViewName.Should().Be("CreateAccount");
            ((CreateAccountViewModel)((ViewResult)result).Model).Username.Should().Be("email+address@company.com");
            ((CreateAccountViewModel)((ViewResult)result).Model).Password.Should().Be("Pa55word");
            ((CreateAccountViewModel)((ViewResult)result).Model).ConfirmPassword.Should().Be("Pa55word");

            Controller.ModelState.Count.Should().Be(1);
            Controller.ModelState.ValidationState.Should().Be(ModelValidationState.Invalid);
            Controller.ModelState.First().Key.Should().Be("Username");
            Controller.ModelState.First().Value.Errors.First().ErrorMessage.Should().Be(_expectedError);
        }
    }
}