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
    public class When_Get_called_on_ConfirmEmail_for_previously_confirmed_token : ConfirmEmailTestBase
    {
        [SetUp]
        public void Arrange()
        {
            Mediator.Send(Arg.Any<VerifyConfirmEmailRequest>()).Returns(new VerifyConfirmEmailResponse() { VerifyConfirmedEmailResult = VerifyConfirmedEmailResult.TokenPreviouslyVerified, Email = "email@email.com" });
            Mediator.Send(Arg.Any<GetClientByReturnUrlRequest>()).Returns(new Client() { ServiceDetails = new ServiceDetails() { ServiceName = "Test Service" } });
        }

        [Test]
        public async Task Then_ViewResult_contains_View_ConfirmEmailSuccessful()
        {
            var result = await Controller.Get(ReturnUrl, IdentityToken);

            result.As<ViewResult>().ViewName.Should().Be("ConfirmEmailAlreadyConfirmed");
        }

        [Test]
        public async Task Then_ViewResult_contains_ConfirmEmailSuccessfulViewModel()
        {
            var result = await Controller.Get(ReturnUrl, IdentityToken);

            result.As<ViewResult>().Model.Should().BeOfType<ConfirmEmailSuccessfulViewModel>();
        }

        [Test]
        public async Task Then_ViewModel_contains_ReturnUrl_And_ServiceName()
        {
            var result = await Controller.Get(ReturnUrl, IdentityToken);
            result.As<ViewResult>().Model.As<ConfirmEmailSuccessfulViewModel>().ReturnUrl.Should().Be(Uri.UnescapeDataString(ReturnUrl));
            result.As<ViewResult>().Model.As<ConfirmEmailSuccessfulViewModel>().ServiceName.Should().Be("Test Service");
        }
    }
}