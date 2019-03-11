using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.CreatePassword
{
    [TestFixture]
    public class When_invitation_resent
    {
        private Guid _invitationId;
        private IMediator _mediator;
        private CreatePasswordController _controller;

        [SetUp]
        public void SetUp()
        {
            _invitationId = Guid.NewGuid();
            _mediator = Substitute.For<IMediator>();
            _mediator.Send(Arg.Is<GetInvitationByIdRequest>(r => r.InvitationId == _invitationId), CancellationToken.None)
                .Returns(new Invitation() {Id = _invitationId, Email = "email@address.com"});
            _controller = new CreatePasswordController(_mediator);
        }
        
        [Test]
        public async Task Then_mediator_is_asked_for_invitation()
        {
            await _controller.Reinvited(_invitationId);

            await _mediator.Received().Send(Arg.Is<GetInvitationByIdRequest>(r => r.InvitationId == _invitationId),
                CancellationToken.None);
        }
        
        [Test]
        public async Task Then_correct_ViewResult_is_returned()
        {         
            var result = await _controller.Reinvited(_invitationId);

            await _mediator.Received().Send(Arg.Is<GetInvitationByIdRequest>(r => r.InvitationId == _invitationId),
                CancellationToken.None);

            result.Should().BeOfType<ViewResult>();
            ((ViewResult) result).ViewName.Should().Be("Reinvited");
            ((ViewResult) result).Model.Should().BeOfType<ReinvitedViewModel>();
            ((ReinvitedViewModel) ((ViewResult) result).Model).Email.Should().Be("email@address.com");
        }
    }
}