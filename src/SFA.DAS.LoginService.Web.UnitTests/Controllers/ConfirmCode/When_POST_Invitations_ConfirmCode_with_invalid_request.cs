using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ConfirmCode;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.ConfirmCode
{
    [TestFixture]
    public class When_POST_Invitations_ConfirmCode_with_invalid_request
    {
        [Test]
        public void Then_ViewResult_to_Confirmcode_is_returned()
        {
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<ConfirmCodeRequest>(), CancellationToken.None).Returns(new ConfirmCodeResponse() {IsValid = false});
            
            var controller = new ConfirmCodeController(mediator);
            var result = controller.Post(new ConfirmCodeViewModel(Guid.NewGuid(), "code")).Result;

            result.Should().BeOfType<ViewResult>();
            ((ViewResult) result).ViewName.Should().Be("ConfirmCode");
        }

        
        [Test]
        public void Then_ViewResult_contains_GetInvitationByIdResponse_ViewModel()
        {
            var invitationId = Guid.NewGuid();
            
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<ConfirmCodeRequest>(), CancellationToken.None).Returns(new ConfirmCodeResponse() {IsValid = false});
            mediator.Send(Arg.Any<GetInvitationByIdRequest>(), CancellationToken.None).Returns(new Invitation
            {
                Id = invitationId,
                ValidUntil = SystemTime.UtcNow().AddHours(1)
            });
            
            var controller = new ConfirmCodeController(mediator);
            
            
            var result = controller.Post(new ConfirmCodeViewModel(invitationId, "code")).Result;

            ((ViewResult) result).Model.Should().BeOfType<ConfirmCodeViewModel>();
            ((ConfirmCodeViewModel) ((ViewResult) result).Model).InvitationId.Should().Be(invitationId);
        }
        
        [Test]
        public void Then_ViewResult_contains_CodeNotValid_error_in_ModelState()
        {
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<ConfirmCodeRequest>(), CancellationToken.None).Returns(new ConfirmCodeResponse() {IsValid = false});
            mediator.Send(Arg.Any<GetInvitationByIdRequest>(), CancellationToken.None).Returns(new Invitation
            {
                Id = Guid.NewGuid(),
                ValidUntil = SystemTime.UtcNow().AddHours(1)
            });
            
            var controller = new ConfirmCodeController(mediator);
            var invitationId = Guid.NewGuid();
            
            controller.Post(new ConfirmCodeViewModel(invitationId, "code")).Wait();

            controller.ModelState.Count.Should().Be(1);
            controller.ModelState.ValidationState.Should().Be(ModelValidationState.Invalid);
            controller.ModelState.First().Key.Should().Be("Code");
            controller.ModelState.First().Value.Errors.First().ErrorMessage.Should().Be("Code not valid");
        }
        
        [Test]
        public void And_code_is_empty_Then_ViewResult_contains_PleaseSupplyCode_error_in_ModelState()
        {
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<ConfirmCodeRequest>(), CancellationToken.None).Returns(new ConfirmCodeResponse() {IsValid = false});
            mediator.Send(Arg.Any<GetInvitationByIdRequest>(), CancellationToken.None).Returns(new Invitation
            {
                Id = Guid.NewGuid(),
                ValidUntil = SystemTime.UtcNow().AddHours(1)
            });
            
            var controller = new ConfirmCodeController(mediator);
            var invitationId = Guid.NewGuid();
            
            controller.Post(new ConfirmCodeViewModel(invitationId, "")).Wait();

            controller.ModelState.Count.Should().Be(1);
            controller.ModelState.ValidationState.Should().Be(ModelValidationState.Invalid);
            controller.ModelState.First().Key.Should().Be("Code");
            controller.ModelState.First().Value.Errors.First().ErrorMessage.Should().Be("Enter confirmation code");
        }
    }
}