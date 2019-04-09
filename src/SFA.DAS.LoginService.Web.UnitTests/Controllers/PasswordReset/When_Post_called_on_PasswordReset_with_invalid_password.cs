using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.PasswordReset
{
    public class When_Post_called_on_PasswordReset_with_invalid_password : PasswordResetTestBase
    {
        private IActionResult _result;

        [SetUp]
        public async Task Act()
        {
            Mediator.Send(Arg.Any<ResetUserPasswordRequest>()).Returns(new ResetPasswordResponse() {IsSuccessful = false});
            _result = await Controller.Post(Guid.NewGuid(), Guid.NewGuid(), new ResetPasswordViewModel
            {
                Password = "one", ConfirmPassword = "one"
            });
        }
        
        [Test]
        public void Then_ModelState_has_an_error_added()
        {
            Controller.ModelState["Password"].Should().NotBeNull();
            Controller.ModelState["Password"].Errors.Count.Should().Be(1);
            Controller.ModelState["Password"].Errors[0].ErrorMessage.Should().Be("Password does not meet minimum complexity requirements");
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