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

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.ConfirmEmail
{
    public class When_EmailConfirmationRequired_called_on_ConfirmEmail : ConfirmEmailTestBase
    {
        [SetUp]
        public void Arrange()
        {
            Mediator.Send(Arg.Is<CheckExistsConfirmEmailRequest>(r => r.Email == "test@test.com")).Returns(new CheckExistsConfirmEmailResponse() { HasRequest = true, IsValid = true });
        }
        
        [Test]
        public async Task Then_request_is_passed_on_to_mediator()
        {
            await Controller.EmailConfirmationRequired("test@test.com", Uri.UnescapeDataString(ReturnUrl));

            await Mediator.Received(1).Send(Arg.Is<CheckExistsConfirmEmailRequest>(r => r.Email == "test@test.com"), CancellationToken.None);
        }

        [Test]
        public async Task Then_ViewResult_is_returned()
        {
            var result = await Controller.EmailConfirmationRequired("test@test.com", Uri.UnescapeDataString(ReturnUrl));

            result.Should().BeOfType<ViewResult>();
        }
    }
}