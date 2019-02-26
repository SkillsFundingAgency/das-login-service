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
    public class When_CreatePassword_called_and_password_is_invalid : CreatePasswordTestsBase
    {
        [Test]
        public void Then_CreatePasswordResponse_PasswordValid_is_false()
        {
            UserService.CreateUser(Arg.Any<LoginUser>()).Returns(new CreateUserResponse(){User = new LoginUser(){Id = NewLoginUserId.ToString()}, Result = IdentityResult.Failed()});
            var response = Handler.Handle(new CreatePasswordRequest {InvitationId = InvitationId, Password = "password"}, CancellationToken.None).Result;
            response.PasswordValid.Should().BeFalse();
        }
        
        [Test]
        public void Then_Invitation_is_not_updated_as_complete()
        {
            UserService.CreateUser(Arg.Any<LoginUser>()).Returns(new CreateUserResponse(){User = new LoginUser(){Id = NewLoginUserId.ToString()}, Result = IdentityResult.Failed()});
            Handler.Handle(new CreatePasswordRequest {InvitationId = InvitationId, Password = "password"}, CancellationToken.None).Wait();

            LoginContext.Invitations.Single(i => i.Id == InvitationId).IsComplete.Should().BeFalse();
        }
        
        [Test]
        public void Then_a_callback_job_is_not_queued()
        {           
            UserService.CreateUser(Arg.Any<LoginUser>()).Returns(new CreateUserResponse(){User = new LoginUser(){Id = NewLoginUserId.ToString()}, Result = IdentityResult.Failed()});
            Handler.Handle(new CreatePasswordRequest {InvitationId = InvitationId, Password = "Password"}, CancellationToken.None).Wait();

            BackgroundJobClient.Verify(x => x.Create(It.Is<Job>(
                job => 
                    job.Method.Name == "Callback"
                    && job.Args[0] is Invitation 
                    && (string)job.Args[1] == NewLoginUserId.ToString()), It.IsAny<EnqueuedState>()), Times.Never);
        }
    }
}