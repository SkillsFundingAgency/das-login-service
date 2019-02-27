using System;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.ConfirmCode
{
    [TestFixture]
    public class When_GET_Invitations_ConfirmCode_for_completed_invitation
    {
        [Test]
        public void Then_expired_view_returned()
        {
            var invitationId = Guid.NewGuid();
            
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<GetInvitationByIdRequest>()).Returns(new InvitationResponse(new Invitation(){IsUserCreated = true}));
            
            var controller = new ConfirmCodeController(mediator);
            
            var result = controller.Get(invitationId).Result;
            
            ((ViewResult) result).ViewName.Should().Be("InvitationExpired");
        }
    }
    
    [TestFixture]
    public class When_GET_Invitations_ConfirmCode_with_id_for_expired_Invitation
    {
        [Test]
        public void Then_Expired_Link_View_Returned()
        {
            var invitationId = Guid.NewGuid();
            
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<GetInvitationByIdRequest>()).Returns(default(InvitationResponse));
            
            var controller = new ConfirmCodeController(mediator);
            
            var result = controller.Get(invitationId).Result;
            
            ((ViewResult) result).ViewName.Should().Be("InvitationExpired");
        }
    }
}