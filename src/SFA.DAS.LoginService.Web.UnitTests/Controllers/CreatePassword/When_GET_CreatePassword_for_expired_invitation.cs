using System;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.CreatePassword
{
    [TestFixture]
    public class When_GET_CreatePassword_for_expired_invitation
    {
        [Test]
        public void Then_ViewResult_for_InvitationExpired_returned()
        {
            SystemTime.UtcNow = () => new DateTime(2019,1,1,1,1,1);
            
            var mediator = Substitute.For<IMediator>();
            var invitationId = Guid.NewGuid();
            mediator.Send(Arg.Any<GetInvitationByIdRequest>()).Returns(new Invitation() {Id = invitationId, ValidUntil = SystemTime.UtcNow().AddMinutes(-1)});
            
            var controller = new CreatePasswordController(mediator);
            var result = controller.Get(invitationId).Result;
            result.Should().BeOfType<ViewResult>();
            result.As<ViewResult>().ViewName.Should().Be("InvitationExpired");
            result.As<ViewResult>().Model.As<InvitationExpiredViewModel>().InvitationId.Should().Be(invitationId);
        }
    }
}