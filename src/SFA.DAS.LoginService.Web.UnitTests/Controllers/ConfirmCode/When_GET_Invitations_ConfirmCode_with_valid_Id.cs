using System;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ConfirmCode;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.ConfirmCode
{
    [TestFixture]
    public class When_GET_Invitations_ConfirmCode_with_valid_Id
    {
        private Guid _invitationId;
        private IMediator _mediator;

        [SetUp]
        public void SetUp()
        {
            _invitationId = Guid.NewGuid();
            var invitation = new Invitation
            {
                Id = _invitationId,
                FamilyName = "Smith",
                GivenName = "James",
                ValidUntil = DateTime.UtcNow.AddHours(1)
            };
            
            _mediator = Substitute.For<IMediator>();
            _mediator.Send(Arg.Any<GetInvitationByIdRequest>()).Returns(new InvitationResponse(invitation));
        }
        
        
        [Test]
        public void Then_ViewResult_Is_Returned()
        {
            var controller = new ConfirmCodeController(_mediator);
            var result = controller.Get(_invitationId).Result;
            result.Should().BeOfType<ViewResult>();
        }

        [Test]
        public void Then_Mediator_is_called_with_the_Id()
        {
            var controller = new ConfirmCodeController(_mediator);
            
            var result = controller.Get(_invitationId).Result;
            ((ViewResult) result).ViewName.Should().Be("ConfirmCode");

            _mediator.Received().Send(Arg.Is<GetInvitationByIdRequest>(r => r.InvitationId == _invitationId));
        }

        [Test]
        public void Then_invitation_response_is_passed_through_to_view()
        {
            var controller = new ConfirmCodeController(_mediator);
            
            var result = controller.Get(_invitationId).Result;

            ((ViewResult) result).Model.Should().BeOfType<ConfirmCodeViewModel>();
        }
    }
}