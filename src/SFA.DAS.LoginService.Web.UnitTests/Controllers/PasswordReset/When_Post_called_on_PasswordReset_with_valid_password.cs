using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Web.Controllers.Password.ViewModels;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.PasswordReset
{
    public class When_Post_called_on_PasswordReset_with_valid_password : PasswordResetTestBase
    {
        private IActionResult _result;
        private Guid _clientId;

        [SetUp]
        public async Task Act()
        {
            Mediator.Send(Arg.Any<ResetUserPasswordRequest>()).Returns(new ResetPasswordResponse() {IsSuccessful = true});
            _clientId = Guid.NewGuid();
            _result = await Controller.Post(_clientId, Guid.NewGuid(), new ResetPasswordViewModel
            {
                Password = "one",
                ConfirmPassword = "one"
            });
        }
        
        [Test]
        public void Then_ModelState_has_no_errors_added()
        {
            Controller.ModelState["Password"].Should().BeNull();    
        }

        [Test]
        public void Then_RedirectToAction_is_returned()
        {
            _result.Should().BeOfType<RedirectToActionResult>();
            _result.As<RedirectToActionResult>().ActionName.Should().Be("PasswordResetSuccessful");
            _result.As<RedirectToActionResult>().RouteValues["clientId"].Should().Be(_clientId);
        }
        
    }
}