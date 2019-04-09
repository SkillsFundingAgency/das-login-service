using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.PasswordReset
{
    public class When_Get_called_on_PasswordReset_for_valid_request : PasswordResetTestBase
    {
        private Guid _requestId;

        [SetUp]
        public void Arrange()
        {
            _requestId = Guid.NewGuid();
            Mediator.Send(Arg.Any<CheckPasswordResetRequest>()).Returns(new CheckPasswordResetResponse() {IsValid = true, Email = "email@emailaddress.com", RequestId = _requestId});
        }

        [Test]
        public async Task Then_ViewResult_contains_View_ResetPassword()
        {
            var result = await Controller.Get(Guid.NewGuid(), _requestId);

            result.As<ViewResult>().ViewName.Should().Be("ResetPassword");
        }

        [Test]
        public async Task Then_ViewResult_contains_ResetPasswordViewModel()
        {
            var clientId = Guid.NewGuid();
            var result = await Controller.Get(clientId, _requestId);

            result.As<ViewResult>().Model.Should().BeOfType<ResetPasswordViewModel>();
            result.As<ViewResult>().Model.As<ResetPasswordViewModel>().RequestId.Should().Be(_requestId);
            result.As<ViewResult>().Model.As<ResetPasswordViewModel>().Password.Should().BeEmpty();
            result.As<ViewResult>().Model.As<ResetPasswordViewModel>().ConfirmPassword.Should().BeEmpty();
            result.As<ViewResult>().Model.As<ResetPasswordViewModel>().ClientId.Should().Be(clientId);
        }
    }
}