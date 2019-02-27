using System;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.CreatePassword;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.CreatePassword
{
    [TestFixture]
    public class When_POST_CreatePassword_with_valid_password
    {
        [Test]
        public void Then_RedirectToActionResult_is_returned()
        {
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<CreatePasswordRequest>()).Returns(new CreatePasswordResponse() {PasswordValid = true});
            mediator.Send(Arg.Any<GetInvitationByIdRequest>()).Returns(new InvitationResponse(new Invitation() {CodeConfirmed = true}));
            var controller = new CreatePasswordController(mediator);

            var invitationId = Guid.NewGuid();
            var result = controller.Post(new CreatePasswordViewModel() {InvitationId = invitationId, Password = "Pa55word", ConfirmPassword = "Pa55word"}).Result;

            result.Should().BeOfType<RedirectToActionResult>();
            ((RedirectToActionResult) result).ActionName.Should().Be("Get");
            ((RedirectToActionResult) result).ControllerName.Should().Be("SignUpComplete");
            ((RedirectToActionResult) result).RouteValues["id"].Should().Be(invitationId);
        }
    }
}