using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Application.Invitations.CreateInvitation;
using SFA.DAS.LoginService.Application.Reinvite;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.CreatePassword
{
    [TestFixture]
    public class When_resend_of_invitation_requested
    {
        [Test]
        public async Task Then_reinvite_handler_is_called()
        {
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<ReinviteRequest>(), CancellationToken.None).Returns(new CreateInvitationResponse()
                {Invited = true, Message = ""});
            
            var controller = new CreatePasswordController(mediator);
            var invitationId = Guid.NewGuid();
            
            await controller.Reinvite(invitationId);
            
            await mediator.Received().Send(Arg.Is<ReinviteRequest>(r => r.InvitationId == invitationId));
        }

        [Test]
        public async Task And_successful_reinvite_Then_RedirectToActionResult()
        {
            var mediator = Substitute.For<IMediator>();
            var newInvitationId = Guid.NewGuid();
            mediator.Send(Arg.Any<ReinviteRequest>(), CancellationToken.None).Returns(new CreateInvitationResponse()
                {Invited = true, Message = "", InvitationId = newInvitationId});
            var controller = new CreatePasswordController(mediator);
            var invitationId = Guid.NewGuid();
            
            var result = await controller.Reinvite(invitationId);

            result.Should().BeOfType<RedirectToActionResult>();
            ((RedirectToActionResult) result).ActionName.Should().Be("Reinvited");
            ((RedirectToActionResult) result).RouteValues["invitationId"].Should().Be(newInvitationId);
        }
        
        [Test]
        public async Task And_unsuccessful_reinvite_with_NonExistingUser_Then_BadRequestResult()
        {
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<ReinviteRequest>(), CancellationToken.None).Returns(new CreateInvitationResponse()
            { Invited = false, ExistingUserId = null, Message = "Error message" }) ;
            var controller = new CreatePasswordController(mediator);
            var invitationId = Guid.NewGuid();
            
            var result = await controller.Reinvite(invitationId);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task And_unsuccessful_reinvite_with_ExistingUser_Then_ViewResult()
        {
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<ReinviteRequest>(), CancellationToken.None).Returns(new CreateInvitationResponse()
            { Invited = false, ExistingUserId = Guid.NewGuid().ToString(), Message = "Error message" });

            mediator.Send(Arg.Any<GetInvitationByIdRequest>(), CancellationToken.None).Returns(new Invitation()
            { ClientId = Guid.NewGuid() });

            var controller = new CreatePasswordController(mediator);
            var invitationId = Guid.NewGuid();

            var result = await controller.Reinvite(invitationId);

            result.Should().BeOfType<ViewResult>();
            ((ViewResult)result).ViewName.Should().Be("AccountExists");
        }
    }
}