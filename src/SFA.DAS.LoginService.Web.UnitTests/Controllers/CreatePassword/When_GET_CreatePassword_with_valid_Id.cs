using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.CreatePassword
{
    [TestFixture]
    public class When_GET_CreatePassword_with_valid_Id
    {
        [Test]
        public void Then_correct_ViewResult_is_returned()
        {
            var controller = new CreatePasswordController();
            var result = controller.Get(Guid.NewGuid()).Result;
            result.Should().BeOfType<ViewResult>();
            ((ViewResult) result).ViewName.Should().Be("CreatePassword");
        }
        
        [Test]
        public void Then_correct_CreatePasswordViewModel_is_passed_to_View()
        {
            var controller = new CreatePasswordController();
            var invitationId = Guid.NewGuid();
            var result = controller.Get(invitationId).Result;

            ((ViewResult) result).Model.Should().BeOfType<CreatePasswordViewModel>();
            ((CreatePasswordViewModel) ((ViewResult) result).Model).InvitationId.Should().Be(invitationId);
            ((CreatePasswordViewModel) ((ViewResult) result).Model).Password.Should().BeEmpty();
            ((CreatePasswordViewModel) ((ViewResult) result).Model).ConfirmPassword.Should().BeEmpty();
        }
    }
}