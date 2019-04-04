using System;
using System.Threading;
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
    public class When_EmailConfirmationRequired_called_on_ConfirmEmail_for_existing_invalid_request : ConfirmEmailTestBase
    {
        [SetUp]
        public void Arrange()
        {
            Mediator.Send(Arg.Is<CheckExistsConfirmEmailRequest>(r => r.Email == "test@test.com")).Returns(new CheckExistsConfirmEmailResponse() { HasRequest = true, IsValid = false });
        }
        
        [Test]
        public async Task Then_request_is_passed_on_to_mediator()
        {
            await Controller.EmailConfirmationRequired("test@test.com", Uri.UnescapeDataString(ReturnUrl));

            await Mediator.Received(1).Send(Arg.Is<CheckExistsConfirmEmailRequest>(r => r.Email == "test@test.com"), CancellationToken.None);
        }

        [Test]
        public async Task Then_ViewResult_contains_View_ConfirmEmailExpiredLink()
        {
            var result = await Controller.EmailConfirmationRequired("test@test.com", Uri.UnescapeDataString(ReturnUrl));

            result.As<ViewResult>().ViewName.Should().Be("ConfirmEmailExpiredLink");
        }

        [Test]
        public async Task Then_ViewModel_contains_Email_And_ReturnUrl()
        {
            var result = await Controller.EmailConfirmationRequired("test@test.com", Uri.UnescapeDataString(ReturnUrl));
            result.As<ViewResult>().Model.As<ConfirmEmailLinkViewModel>().Email.Should().Be("test@test.com");
            result.As<ViewResult>().Model.As<ConfirmEmailLinkViewModel>().ReturnUrl.Should().Be(Uri.UnescapeDataString(ReturnUrl));
        }
    }
}