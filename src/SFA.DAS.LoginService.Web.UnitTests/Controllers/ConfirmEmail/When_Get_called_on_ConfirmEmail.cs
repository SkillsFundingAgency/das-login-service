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
    public class When_Get_called_on_ConfirmEmail : ConfirmEmailTestBase
    {
        [SetUp]
        public void Arrange()
        {
            Mediator.Send(Arg.Any<VerifyConfirmEmailRequest>()).Returns(new VerifyConfirmEmailResponse() { VerifyConfirmedEmailResult = VerifyConfirmedEmailResult.TokenVerified, Email = "email@emailaddress.com"});
            Mediator.Send(Arg.Any<GetClientByReturnUrlRequest>()).Returns(new Client() { ServiceDetails = new ServiceDetails() { ServiceName = "Test Service" } });
        }
        
        [Test]
        public async Task Then_request_is_passed_on_to_mediator()
        {
            await Controller.Get(ReturnUrl, IdentityToken);

            await Mediator.Received().Send(Arg.Is<VerifyConfirmEmailRequest>(r => r.IdentityToken == Uri.UnescapeDataString(IdentityToken)), CancellationToken.None);
        }

        [Test]
        public async Task Then_ViewResult_is_returned()
        {
            var result = await Controller.Get(ReturnUrl, IdentityToken);

            result.Should().BeOfType<ViewResult>();
        }
    }
}