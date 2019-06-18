using System;
using System.Linq;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
    public class When_POST_CreatePassword_with_invalid_password
    {
        [Test]
        public void Then_ViewResult_is_returned()
        {
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<CreatePasswordRequest>()).Returns(new CreatePasswordResponse() {PasswordValid = false});
            mediator.Send(Arg.Any<GetInvitationByIdRequest>()).Returns(new Invitation() );
            var controller = new CreatePasswordController(mediator);

            var invitationId = Guid.NewGuid();
            var result = controller.Post(new CreatePasswordViewModel() {InvitationId = invitationId, Password = "Pa55word", ConfirmPassword = "Pa55word"}).Result;
            
            result.Should().BeOfType<ViewResult>();
            ((ViewResult) result).ViewName.Should().Be("CreatePassword");
            ((CreatePasswordViewModel) ((ViewResult) result).Model).Password.Should().Be("Pa55word");
            ((CreatePasswordViewModel) ((ViewResult) result).Model).ConfirmPassword.Should().Be("Pa55word");
            
            
            controller.ModelState.Count.Should().Be(1);
            controller.ModelState.ValidationState.Should().Be(ModelValidationState.Invalid);
            controller.ModelState.First().Key.Should().Be("Password");
            controller.ModelState.First().Value.Errors.First().ErrorMessage.Should().Be("Password does not meet minimum complexity requirements");
        }
    }
}