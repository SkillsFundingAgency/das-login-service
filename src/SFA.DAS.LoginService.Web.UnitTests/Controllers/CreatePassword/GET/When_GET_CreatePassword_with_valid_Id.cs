using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.CreatePassword;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.CreatePassword
{
    public class When_GET_CreatePassword_with_valid_Id : CreatePasswordTestsBase
    {
        [Test]
        public void Then_correct_ViewResult_is_returned()
        {
            Mediator.Send(Arg.Any<GetInvitationByIdRequest>()).Returns(new Invitation() {ValidUntil = DateTime.Now.AddHours(1)});
            
            var result = Controller.Get(Guid.NewGuid()).Result;
            result.Should().BeOfType<ViewResult>();
            ((ViewResult) result).ViewName.Should().Be("CreatePassword");
        }
        
        [Test]
        public void Then_correct_CreatePasswordViewModel_is_passed_to_View()
        {
            Mediator.Send(Arg.Any<GetInvitationByIdRequest>()).Returns(new Invitation() {ValidUntil = DateTime.Now.AddHours(1), Email = "email@email.com"});
            
            var invitationId = Guid.NewGuid();
            var result = Controller.Get(invitationId).Result;

            ((ViewResult) result).Model.Should().BeOfType<CreatePasswordViewModel>();
            ((CreatePasswordViewModel) ((ViewResult) result).Model).InvitationId.Should().Be(invitationId);
            ((CreatePasswordViewModel) ((ViewResult) result).Model).Username.Should().Be("email@email.com");
            ((CreatePasswordViewModel) ((ViewResult) result).Model).Password.Should().BeEmpty();
            ((CreatePasswordViewModel) ((ViewResult) result).Model).ConfirmPassword.Should().BeEmpty();
        }
    }
}