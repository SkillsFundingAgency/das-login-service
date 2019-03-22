using System;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Invitations.CreateInvitation;
using SFA.DAS.LoginService.Web.Controllers.InvitationsApi;
using SFA.DAS.LoginService.Web.Controllers.InvitationsApi.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.Invitations
{
    [TestFixture]
    public class When_invitation_called
    {
        private IMediator _mediator;
        private InvitationsController _controller;
        private Guid _sourceId;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();
            _controller = new InvitationsController(_mediator, Substitute.For<ILogger<InvitationsController>>());

            _sourceId = Guid.NewGuid();
        }
        
        [Test]
        public void Then_request_is_passed_on_to_mediator()
        {
            _mediator.Send(Arg.Any<CreateInvitationRequest>()).Returns(new CreateInvitationResponse() {Invited = true});
            
            var invitationRequest = new InvitationRequestViewModel()
            {
                Email = "email@email.com",
                GivenName = "Dave",
                FamilyName = "Smith",
                SourceId = _sourceId.ToString(),
                Callback = new Uri("https://callback"),
                UserRedirect = new Uri("https://userRedirect")
            };

            var clientId = Guid.NewGuid();
            _controller.Invite(clientId, invitationRequest).Wait();

            _mediator.Received().Send(Arg.Is<CreateInvitationRequest>(r => r.Email == "email@email.com"
                                                                          && r.GivenName == "Dave"
                                                                          && r.FamilyName == "Smith"
                                                                          && r.SourceId == _sourceId.ToString()
                                                                          && r.Callback == new Uri("https://callback")
                                                                          && r.UserRedirect == new Uri("https://userRedirect")
                                                                          && r.ClientId == clientId));
        }

        [Test]
        public void And_request_arguments_are_invalid_Then_BadRequest_should_be_returned()
        {
            _mediator.Send(Arg.Any<CreateInvitationRequest>()).Throws(new ArgumentException("Email, FamilyName"));
            
            var invitationRequest = new InvitationRequestViewModel()
            {
                Email = "email@email.com",
                GivenName = "Dave",
                FamilyName = "Smith",
                SourceId = _sourceId.ToString(),
                Callback = new Uri("https://callback"),
                UserRedirect = new Uri("https://userRedirect")
            };
            
            var clientId = Guid.NewGuid();
            var result = _controller.Invite(clientId, invitationRequest).Result;

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Test]
        public void And_Invitation_is_valid_Then_CreateInvitationResponse_Is_Returned()
        {
            _mediator.Send(Arg.Any<CreateInvitationRequest>()).Returns(new CreateInvitationResponse() {Invited = true});

            var invitationRequest = new InvitationRequestViewModel()
            {
                Email = "email@email.com",
                GivenName = "Dave",
                FamilyName = "Smith",
                SourceId = _sourceId.ToString(),
                Callback = new Uri("https://callback"),
                UserRedirect = new Uri("https://userRedirect")
            };
            var clientId = Guid.NewGuid();
            var response = _controller.Invite(clientId, invitationRequest).Result;

            response.Value.Should().BeOfType<CreateInvitationResponse>();
            response.Value.Invited.Should().BeTrue();
        }
    }
}