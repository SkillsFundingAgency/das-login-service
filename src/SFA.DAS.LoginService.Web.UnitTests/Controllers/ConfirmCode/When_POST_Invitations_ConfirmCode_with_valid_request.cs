using System;
using System.Threading;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ConfirmCode;
using SFA.DAS.LoginService.Web.Controllers.InvitationWeb;

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
            mediator.Send(Arg.Any<ConfirmCodeViewModel>(), CancellationToken.None).Returns(new ConfirmCodeResponse());
            
            var controller = new ConfirmCodeController(mediator);
            
            var confirmCodeRequest = new ConfirmCodeViewModel(invitationId, "code");
            controller.Post(confirmCodeRequest).Wait();

            mediator.Received().Send(confirmCodeRequest);
        }
        
        [Test]
        public void Then_redirect_to_password_page_is_returned()
        {
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<ConfirmCodeViewModel>(), CancellationToken.None).Returns(new ConfirmCodeResponse() {IsValid = true});
            
            var controller = new ConfirmCodeController(mediator);
            var result = controller.Post(new ConfirmCodeViewModel(Guid.NewGuid(), "code")).Result;

            result.Should().BeOfType<RedirectToActionResult>();
            ((RedirectToActionResult) result).ActionName.Should().Be("Get");
            ((RedirectToActionResult) result).ControllerName.Should().Be("CreatePassword");
        }
    }
}