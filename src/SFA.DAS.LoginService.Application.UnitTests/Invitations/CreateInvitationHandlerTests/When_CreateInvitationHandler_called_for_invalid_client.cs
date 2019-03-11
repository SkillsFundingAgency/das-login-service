using System;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.CreateInvitationHandlerTests
{
    public class When_CreateInvitationHandler_called_for_invalid_client : CreateInvitationHandlerTestBase
    {
        [Test]
        public void Then_Invited_is_false()
        {
            var invalidClientId = Guid.NewGuid();
            LoginContext.Clients.Add(new Client(){Id = invalidClientId, AllowInvitationSignUp = false});
            LoginContext.SaveChanges();
            
            var createInvitationRequest = BuildCreateInvitationRequest();
            createInvitationRequest.ClientId = invalidClientId;
            var result = CreateInvitationHandler.Handle(createInvitationRequest, CancellationToken.None).Result;
            result.Invited.Should().BeFalse();
            result.Message.Should().Be("Client is not authorised for Invitiation Signup");
        }
    }
}