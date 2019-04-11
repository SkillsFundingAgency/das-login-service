using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.LoginService.Web.Controllers.Password.ViewModels;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.PasswordReset
{
    public class When_Post_called_with_invalid_ModelState : PasswordResetTestBase
    {
        private IActionResult _result;

        [SetUp]
        public async Task Act()
        {
            Controller.ModelState.AddModelError("PasswordViewModel.Password", "Must not be blank");
            _result = await Controller.Post(Guid.NewGuid(), Guid.NewGuid(), new ResetPasswordViewModel
            {
                Password = "one",
                ConfirmPassword = "two"
            });
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