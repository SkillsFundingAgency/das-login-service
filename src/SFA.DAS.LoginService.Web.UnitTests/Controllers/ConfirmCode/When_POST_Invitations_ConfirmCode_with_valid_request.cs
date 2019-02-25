using System;
using System.Threading;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ConfirmCode;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.ConfirmCode
{
    [TestFixture]
    public class When_POST_Invitations_ConfirmCode_with_valid_request
    {
        [Test]
        public void Then_request_is_passed_through_to_mediator()
        {
            var invitationId = Guid.NewGuid();
            
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<ConfirmCodeRequest>(), CancellationToken.None).Returns(new ConfirmCodeResponse());
            
            var controller = new ConfirmCodeController(mediator);
            
            var confirmCodeViewModel = new ConfirmCodeViewModel(invitationId, "code");
            controller.Post(confirmCodeViewModel).Wait();

            mediator.Received().Send(Arg.Is<ConfirmCodeRequest>(r => r.InvitationId == invitationId));
        }
        
        [Test]
        public void Then_redirect_to_password_page_is_returned()
        {
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<ConfirmCodeRequest>(), CancellationToken.None).Returns(new ConfirmCodeResponse() {IsValid = true});
            
            var controller = new ConfirmCodeController(mediator);
            var result = controller.Post(new ConfirmCodeViewModel(Guid.NewGuid(), "code")).Result;

            result.Should().BeOfType<RedirectToActionResult>();
            ((RedirectToActionResult) result).ActionName.Should().Be("Get");
            ((RedirectToActionResult) result).ControllerName.Should().Be("CreatePassword");
        }
    }
}