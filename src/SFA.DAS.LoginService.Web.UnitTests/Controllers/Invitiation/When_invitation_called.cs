using System;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Invitations.CreateInvitation;
using SFA.DAS.LoginService.Web.Controllers;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.Invitiation
{
    [TestFixture]
    public class When_invitation_called
    {
        [Test]
        public void Then_request_is_passed_on_to_mediator()
        {
            var mediator = Substitute.For<IMediator>();
            var controller = new InvitationsController(mediator);

            var sourceId = Guid.NewGuid();

            var createInvitationRequest = new CreateInvitationRequest()
            {
                Email = "email@email.com",
                GivenName = "Dave",
                FamilyName = "Smith",
                SourceId = sourceId.ToString(),
                Callback = new Uri("https://callback"),
                UserRedirect = "https://userRedirect"
            };
            controller.Invite(createInvitationRequest).Wait();

            mediator.Received().Send(createInvitationRequest);
        }

        [Test]
        public void And_request_arguments_are_invalid_Then_BadRequest_should_be_returned()
        {
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<CreateInvitationRequest>()).Throws(new ArgumentException("Email, FamilyName"));
            var controller = new InvitationsController(mediator);
            
            var sourceId = Guid.NewGuid();
            
            var createInvitationRequest = new CreateInvitationRequest()
            {
                Email = "email@email.com",
                GivenName = "Dave",
                FamilyName = "Smith",
                SourceId = sourceId.ToString(),
                Callback = new Uri("https://callback"),
                UserRedirect = "https://userRedirect"
            };
            var result = controller.Invite(createInvitationRequest).Result;

            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}