using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ConfirmEmail;
using SFA.DAS.LoginService.Application.GetClientByReturnUrl;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Data.JsonObjects;
using SFA.DAS.LoginService.Web.Controllers.ConfirmEmail.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.ConfirmEmail
{
    public class When_Get_called_on_ConfirmEmail_for_expired_token : ConfirmEmailTestBase
    {
        [SetUp]
        public void Arrange()
        {
            Mediator.Send(Arg.Any<VerifyConfirmEmailRequest>()).Returns(new VerifyConfirmEmailResponse() { VerifyConfirmedEmailResult = VerifyConfirmedEmailResult.TokenExpired, Email = "email@email.com" });
        }

        [Test]
        public async Task Then_ViewResult_contains_View_ConfirmEmailExpiredLink()
        {
            var result = await Controller.Get(ReturnUrl, IdentityToken);

            result.As<ViewResult>().ViewName.Should().Be("ConfirmEmailExpiredLink");
        }

        [Test]
        public async Task Then_ViewResult_contains_ConfirmEmailLinkViewModel()
        {
            var result = await Controller.Get(ReturnUrl, IdentityToken);

            result.As<ViewResult>().Model.Should().BeOfType<ConfirmEmailLinkViewModel>();
        }

        [Test]
        public async Task Then_ViewModel_contains_Email_And_ReturnUrl()
        {
            var result = await Controller.Get(ReturnUrl, IdentityToken);
            result.As<ViewResult>().Model.As<ConfirmEmailLinkViewModel>().Email.Should().Be("email@email.com");
            result.As<ViewResult>().Model.As<ConfirmEmailLinkViewModel>().ReturnUrl.Should().Be(Uri.UnescapeDataString(ReturnUrl));    
        }
    }
}