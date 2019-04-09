using System;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.CreatePassword;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.CreatePassword
{
    [TestFixture]
    public class When_POST_CreatePassword_with_matching_passwords
    {
        [Test]
        public void Then_mediator_is_called_with_correct_request()
        {
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<CreatePasswordRequest>()).Returns(new CreatePasswordResponse() {PasswordValid = true});
            mediator.Send(Arg.Any<GetInvitationByIdRequest>()).Returns(new Invitation() );
            var controller = new CreatePasswordController(mediator);

            var invitationId = Guid.NewGuid();
            controller.Post(new CreatePasswordViewModel() {InvitationId = invitationId, Password = "Pa55word", ConfirmPassword = "Pa55word"}).Wait();

            mediator.Received().Send(Arg.Is<CreatePasswordRequest>(r => r.InvitationId == invitationId && r.Password == "Pa55word"));
        }
    }
}