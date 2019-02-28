using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Application.Invitations.CreateInvitation;
using SFA.DAS.LoginService.Application.Reinvite;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.Reinvite
{
    [TestFixture]
    public class When_Reinvite_called_with_valid_invitation
    {
        private IMediator _mediator;
        private Guid _clientId;
        private ReinviteHandler _handler;
        private Guid _invitationId;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();
            _clientId = Guid.NewGuid();
            
            _mediator.Send(Arg.Any<GetInvitationByIdRequest>(), CancellationToken.None).Returns(new Invitation()
            {
                Email = "email@address.com", 
                FamilyName = "Smith", 
                GivenName = "John", 
                SourceId = "SOURCE123",
                CallbackUri = new Uri("https://callback"), 
                UserRedirectUri = new Uri("https://redirect"),
                ClientId = _clientId
            });

            _mediator.Send(Arg.Any<CreateInvitationRequest>(), CancellationToken.None)
                .Returns(new CreateInvitationResponse() {Invited = true, Message = ""});
            
            _handler = new ReinviteHandler(_mediator);
            _invitationId = Guid.NewGuid();
        }
        
        [Test]
        public async Task Then_mediator_is_asked_for_invitation()
        {
            await _handler.Handle(new ReinviteRequest() {InvitationId = _invitationId}, CancellationToken.None);
            
            await _mediator.Received().Send(Arg.Is<GetInvitationByIdRequest>(r => r.InvitationId == _invitationId));
        }

        [Test]
        public async Task Then_mediator_is_asked_to_create_new_invitation()
        {
            await _handler.Handle(new ReinviteRequest() {InvitationId = _invitationId}, CancellationToken.None);

            await _mediator.Received().Send(Arg.Is<CreateInvitationRequest>(r =>
                r.Email == "email@address.com" 
                && r.FamilyName == "Smith" 
                && r.GivenName == "John" 
                && r.SourceId == "SOURCE123" 
                && r.Callback == new Uri("https://callback")
                && r.UserRedirect == new Uri("https://redirect")
                && r.ClientId == _clientId));
        }

        [Test]
        public async Task Then_correct_CreateInvitationResponse_is_returned()
        {
            var response = await _handler.Handle(new ReinviteRequest() {InvitationId = _invitationId}, CancellationToken.None);
            response.Invited.Should().BeTrue();
            response.Message.Should().BeEmpty();
        }
    }
}