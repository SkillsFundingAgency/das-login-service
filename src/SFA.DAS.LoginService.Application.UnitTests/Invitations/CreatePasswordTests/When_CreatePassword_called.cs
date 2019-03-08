using System.Linq;
using System.Threading;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.CreatePassword;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.CreatePasswordTests
{
    public class When_CreatePassword_called : CreatePasswordTestsBase
    {
        [Test]
        public void Then_UserService_Create_user_is_called()
        {
            UserService.CreateUser(Arg.Any<LoginUser>(), Arg.Any<string>()).Returns(new UserResponse(){User = new LoginUser(){Id = NewLoginUserId.ToString()}, Result = IdentityResult.Success});
            Handler.Handle(new CreatePasswordRequest {InvitationId = InvitationId, Password = "Password"}, CancellationToken.None).Wait();

            UserService.Received().CreateUser(Arg.Is<LoginUser>(u => u.UserName == "email@provider.com" && u.Email == "email@provider.com"), "Password");
        }
        
        [Test]
        public void Then_Invitation_is_updated_to_IsComplete_true()
        {
            UserService.CreateUser(Arg.Any<LoginUser>(), Arg.Any<string>()).Returns(new UserResponse(){User = new LoginUser(){Id = NewLoginUserId.ToString()}, Result = IdentityResult.Success});
            Handler.Handle(new CreatePasswordRequest {InvitationId = InvitationId, Password = "Password"}, CancellationToken.None).Wait();

            var invitation = LoginContext.Invitations.Single(i => i.Id == InvitationId);
            invitation.IsUserCreated.Should().BeTrue();
        }

        [Test]
        public void Then_callback_service_is_called()
        {           
            UserService.CreateUser(Arg.Any<LoginUser>(), Arg.Any<string>()).Returns(new UserResponse(){User = new LoginUser(){Id = NewLoginUserId.ToString()}, Result = IdentityResult.Success});
            Handler.Handle(new CreatePasswordRequest {InvitationId = InvitationId, Password = "Password"}, CancellationToken.None).Wait();

            CallbackService.Received().Callback(Arg.Is<Invitation>(i => i.SourceId == "ABC123"), NewLoginUserId.ToString());
        }
        
        [Test]
        public void Then_CreatePasswordResponse_PasswordValid_is_true()
        {
            UserService.CreateUser(Arg.Any<LoginUser>(), Arg.Any<string>()).Returns(new UserResponse(){User = new LoginUser(){Id = NewLoginUserId.ToString()}, Result = IdentityResult.Success});
            var response = Handler.Handle(new CreatePasswordRequest {InvitationId = InvitationId, Password = "password"}, CancellationToken.None).Result;
            response.PasswordValid.Should().BeTrue();
        }
    }
}