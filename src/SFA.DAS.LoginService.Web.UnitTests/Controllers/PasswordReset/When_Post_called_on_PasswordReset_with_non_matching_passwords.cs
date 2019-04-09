using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.PasswordReset
{
    public class When_Post_called_on_PasswordReset_with_non_matching_passwords : PasswordResetTestBase
    {
        private IActionResult _result;

        [SetUp]
        public async Task Act()
        {
            _result = await Controller.Post(Guid.NewGuid(), Guid.NewGuid(), new ResetPasswordViewModel
            {
                Password = "one", ConfirmPassword = "two"
            });
        }
        
        [Test]
        public void Then_ModelState_has_an_error_added()
        {
            Controller.ModelState["ConfirmPassword"].Should().NotBeNull();
            Controller.ModelState["ConfirmPassword"].Errors.Count.Should().Be(1);
            Controller.ModelState["ConfirmPassword"].Errors[0].ErrorMessage.Should().Be("Passwords should match");
        }

        [Test]
        public void Then_ViewResult_is_returned()
        {
            _result.Should().BeOfType<ViewResult>();
            _result.As<ViewResult>().ViewName.Should().Be("ResetPassword");
            _result.As<ViewResult>().Model.Should().BeOfType<ResetPasswordViewModel>();
        }
    }
}