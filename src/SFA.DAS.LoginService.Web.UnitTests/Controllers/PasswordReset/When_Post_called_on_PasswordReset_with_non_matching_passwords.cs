using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NUnit.Framework;
using SFA.DAS.LoginService.Web.Controllers.Password.ViewModels;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.PasswordReset
{
    public class When_Post_called_on_PasswordReset_with_non_matching_passwords : PasswordResetTestBase
    {
        private IActionResult _result;

        [SetUp]
        public async Task Act()
        {
            // the actual values passed in the view model are not validated by the controller action, this is a test of 
            // the controller when it is called with a model state containing an error
            Controller.ModelState.AddModelError("ConfirmPassword", "Passwords should match");

            _result = await Controller.Post(Guid.NewGuid(), Guid.NewGuid(), new ResetPasswordViewModel
            {
                Password = "one",
                ConfirmPassword = "two"
            });
        }

        [Test]
        public void Then_ResetPassword_ViewResult_is_returned()
        {
            _result.Should().BeOfType<ViewResult>();
            ((ViewResult)_result).ViewName.Should().Be("ResetPassword");
        }

        [Test]
        public void Then_ResetPassword_ViewResult_contains_ResetPasswordViewModel()
        {
            ((ViewResult)_result).Model.Should().BeOfType<ResetPasswordViewModel>();
            ((ResetPasswordViewModel)((ViewResult)_result).Model).Password.Should().Be("one");
            ((ResetPasswordViewModel)((ViewResult)_result).Model).ConfirmPassword.Should().Be("two");
        }

        [Test]
        public void Then_ModelState_has_an_error_added()
        {
            Controller.ModelState.Count.Should().Be(1);
            Controller.ModelState.ValidationState.Should().Be(ModelValidationState.Invalid);
            Controller.ModelState.First().Key.Should().Be("ConfirmPassword");
            Controller.ModelState.First().Value.Errors.First().ErrorMessage.Should().Be("Passwords should match");
        }
    }
}