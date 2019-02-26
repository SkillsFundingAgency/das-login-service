using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluentAssertions;
using Hangfire.Common;
using Hangfire.States;
using Microsoft.AspNetCore.Identity;
using Moq;
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
            UserService.CreateUser(Arg.Any<LoginUser>()).Returns(new CreateUserResponse(){User = new LoginUser(){Id = NewLoginUserId.ToString()}, Result = IdentityResult.Success});
            Handler.Handle(new CreatePasswordRequest {InvitationId = InvitationId, Password = "Password"}, CancellationToken.None).Wait();

            UserService.Received().CreateUser(Arg.Is<LoginUser>(u => u.UserName == "email@provider.com" && u.Email == "email@provider.com"));
        }
        
        [Test]
        public void Then_Invitation_is_updated_to_IsComplete_true()
        {
            UserService.CreateUser(Arg.Any<LoginUser>()).Returns(new CreateUserResponse(){User = new LoginUser(){Id = NewLoginUserId.ToString()}, Result = IdentityResult.Success});
            Handler.Handle(new CreatePasswordRequest {InvitationId = InvitationId, Password = "Password"}, CancellationToken.None).Wait();

            var invitation = LoginContext.Invitations.Single(i => i.Id == InvitationId);
            invitation.IsComplete.Should().BeTrue();
        }

        [Test]
        public void Then_a_callback_job_is_queued()
        {           
            UserService.CreateUser(Arg.Any<LoginUser>()).Returns(new CreateUserResponse(){User = new LoginUser(){Id = NewLoginUserId.ToString()}, Result = IdentityResult.Success});
            Handler.Handle(new CreatePasswordRequest {InvitationId = InvitationId, Password = "Password"}, CancellationToken.None).Wait();

            BackgroundJobClient.Verify(x => x.Create(It.Is<Job>(
                job => 
                    job.Method.Name == "Callback"
                    && job.Args[0] is Invitation 
                    && (string)job.Args[1] == NewLoginUserId.ToString()), It.IsAny<EnqueuedState>()));
        }
        
        [Test]
        public void Then_CreatePasswordResponse_PasswordValid_is_true()
        {
            UserService.CreateUser(Arg.Any<LoginUser>()).Returns(new CreateUserResponse(){User = new LoginUser(){Id = NewLoginUserId.ToString()}, Result = IdentityResult.Success});
            var response = Handler.Handle(new CreatePasswordRequest {InvitationId = InvitationId, Password = "password"}, CancellationToken.None).Result;
            response.PasswordValid.Should().BeTrue();
        }
    }
}