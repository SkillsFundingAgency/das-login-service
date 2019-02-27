using System;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Invitations.CreateInvitation;
using SFA.DAS.LoginService.Web.Controllers;
using SFA.DAS.LoginService.Web.Controllers.InvitationsApi;
using SFA.DAS.LoginService.Web.Controllers.InvitationsApi.ViewModels;

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

            var invitationRequest = new InvitationRequestViewModel()
            {
                Email = "email@email.com",
                GivenName = "Dave",
                FamilyName = "Smith",
                SourceId = sourceId.ToString(),
                Callback = new Uri("https://callback"),
                UserRedirect = new Uri("https://userRedirect")
            };

            var clientId = Guid.NewGuid();
            controller.Invite(clientId, invitationRequest).Wait();

            var createInvitationRequest = new CreateInvitationRequest()
            {
                Email = "email@email.com",
                GivenName = "Dave",
                FamilyName = "Smith",
                SourceId = sourceId.ToString(),
                Callback = new Uri("https://callback"),
                UserRedirect = new Uri("https://userRedirect")
            };
            
            mediator.Received().Send(Arg.Is<CreateInvitationRequest>(r => r.Email == "email@email.com"
                                                                          && r.GivenName == "Dave"
                                                                          && r.FamilyName == "Smith"
                                                                          && r.SourceId == sourceId.ToString()
                                                                          && r.Callback == new Uri("https://callback")
                                                                          && r.UserRedirect == new Uri("https://userRedirect")
                                                                          && r.ClientId == clientId));
        }

        [Test]
        public void And_request_arguments_are_invalid_Then_BadRequest_should_be_returned()
        {
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<CreateInvitationRequest>()).Throws(new ArgumentException("Email, FamilyName"));
            var controller = new InvitationsController(mediator);
            
            var sourceId = Guid.NewGuid();
            
            var invitationRequest = new InvitationRequestViewModel()
            {
                Email = "email@email.com",
                GivenName = "Dave",
                FamilyName = "Smith",
                SourceId = sourceId.ToString(),
                Callback = new Uri("https://callback"),
                UserRedirect = new Uri("https://userRedirect")
            };
            
            var clientId = Guid.NewGuid();
            var result = controller.Invite(clientId, invitationRequest).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public void And_Invitation_is_valid_Then_CreateInvitationResponse_Is_Returned()
        {
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<CreateInvitationRequest>()).Returns(new CreateInvitationResponse() {Invited = true});
            var controller = new InvitationsController(mediator);

            var sourceId = Guid.NewGuid();

            var invitationRequest = new InvitationRequestViewModel()
            {
                Email = "email@email.com",
                GivenName = "Dave",
                FamilyName = "Smith",
                SourceId = sourceId.ToString(),
                Callback = new Uri("https://callback"),
                UserRedirect = new Uri("https://userRedirect")
            };
            var clientId = Guid.NewGuid();
            var response = controller.Invite(clientId, invitationRequest).Result;

            response.Value.Should().BeOfType<CreateInvitationResponse>();
            response.Value.Invited.Should().BeTrue();
        }
    }
}