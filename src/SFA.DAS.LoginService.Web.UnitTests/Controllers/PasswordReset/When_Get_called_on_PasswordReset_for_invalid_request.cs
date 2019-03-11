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
    public class When_Get_called_on_PasswordReset_for_invalid_request : PasswordResetTestBase
    {
        private Guid _requestId;

        [SetUp]
        public void Arrange()
        {
            _requestId = Guid.NewGuid();
            Mediator.Send(Arg.Any<CheckPasswordResetRequest>()).Returns(new CheckPasswordResetResponse() {IsValid = false});
        }

        [Test]
        public async Task Then_ViewResult_contains_View_ExpiredLink()
        {
            var result = await Controller.Get(Guid.NewGuid(), _requestId);

            result.As<ViewResult>().ViewName.Should().Be("~/Views/ConfirmResetCode/ExpiredLink.cshtml");
        }
        
        [Test]
        public async Task Then_ViewResult_contains_ExpiredLinkViewModel()
        {
            var result = await Controller.Get(Guid.NewGuid(), _requestId);

            result.As<ViewResult>().Model.Should().BeOfType<ExpiredLinkViewModel>();
        }

        [Test]
        public async Task Then_ViewModel_contains_clientId()
        {
            var clientId = Guid.NewGuid();
            var result = await Controller.Get(clientId, _requestId);
            result.As<ViewResult>().Model.As<ExpiredLinkViewModel>().ClientId.Should().Be(clientId);
        }
    }
}