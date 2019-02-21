using System;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Invitations.CreateInvitation;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.CreateInvitationHandlerTests
{
    public class When_CreateInvitationHandler_called : CreateInvitationHandlerTestBase
    {
        [Test]
        public void Then_CreateInvitationResult_is_returned()
        {
            var createInvitationRequest = BuildCreateInvitationRequest();
            var result = CreateInvitationHandler.Handle(createInvitationRequest, CancellationToken.None).Result;
            result.Should().BeOfType<CreateInvitationResponse>();
        }

        [Test]
        public void And_Request_arguments_missing_Then_throw_ArgumentException()
        {
            var createInvitationRequest = BuildEmptyCreateInvitationRequest();
            CreateInvitationHandler.Invoking(h => { h.Handle(createInvitationRequest, CancellationToken.None).Wait();}).Should().Throw<ArgumentException>();
        }
    }
}