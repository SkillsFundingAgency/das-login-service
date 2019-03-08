using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ResetPassword;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.PasswordReset
{
    public class When_Get_called_on_PasswordReset : PasswordResetTestBase
    {
        private Guid _requestId;

        [SetUp]
        public void Arrange()
        {
            _requestId = Guid.NewGuid();
            Mediator.Send(Arg.Any<CheckPasswordResetRequest>()).Returns(new CheckPasswordResetResponse() {IsValid = true, Email = "email@emailaddress.com", RequestId = _requestId});
        }
        
        [Test]
        public async Task Then_request_is_passed_on_to_mediator()
        {
            var clientId = Guid.NewGuid();
            
            await Controller.Get(clientId, _requestId);

            await Mediator.Received().Send(Arg.Is<CheckPasswordResetRequest>(r => r.RequestId == _requestId), CancellationToken.None);
        }

        [Test]
        public async Task Then_ViewResult_is_returned()
        {
            var clientId = Guid.NewGuid();
            var requestId = Guid.NewGuid();
            var result = await Controller.Get(clientId, requestId);

            result.Should().BeOfType<ViewResult>();
        }
    }
}