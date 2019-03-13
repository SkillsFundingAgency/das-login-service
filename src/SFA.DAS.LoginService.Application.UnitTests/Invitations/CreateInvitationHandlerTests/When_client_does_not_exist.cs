using System;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.CreateInvitationHandlerTests
{
    public class When_client_does_not_exist : CreateInvitationHandlerTestBase
    {
        [Test]
        public void Then_response_states_that_client_does_not_exist()
        {
            var createInvitationRequest = BuildCreateInvitationRequest();
            createInvitationRequest.ClientId = Guid.NewGuid();
            var response = CreateInvitationHandler.Handle(createInvitationRequest, CancellationToken.None).Result;

            response.Message.Should().Be("Client does not exist");
            response.Invited.Should().Be(false);
        }
    }
}