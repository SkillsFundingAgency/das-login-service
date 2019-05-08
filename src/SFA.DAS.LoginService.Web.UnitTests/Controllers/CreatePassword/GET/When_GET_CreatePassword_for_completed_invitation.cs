using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.CreatePassword
{
    public class When_GET_CreatePassword_for_completed_invitation : CreatePasswordTestsBase
    {
        [Test]
        public void Then_ViewResult_for_InvitationExpired_returned()
        {
            var invitationId = Guid.NewGuid();
            Mediator.Send(Arg.Any<GetInvitationByIdRequest>()).Returns(new Invitation() {Id = invitationId, IsUserCreated = true, ValidUntil = SystemTime.UtcNow().AddHours(1)});
            
            var result = Controller.Get(invitationId).Result;
            result.Should().BeOfType<ViewResult>();
            result.As<ViewResult>().ViewName.Should().Be("InvitationExpired");
            result.As<ViewResult>().Model.As<InvitationExpiredViewModel>().InvitationId.Should().Be(invitationId);
        }
    }
}