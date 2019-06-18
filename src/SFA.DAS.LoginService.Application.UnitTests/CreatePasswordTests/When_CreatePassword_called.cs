using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.CreatePassword;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Invitations.CreateInvitation;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.CreatePasswordTests
{
    public class When_CreatePassword_called : CreatePasswordTestsBase
    {
        [SetUp]
        public void Arrange()
        {
            UserService.CreateUser(Arg.Any<LoginUser>(), Arg.Any<string>()).Returns(new UserResponse(){User = new LoginUser(){Id = NewLoginUserId.ToString()}, Result = IdentityResult.Success});
        }
        
        [Test]
        public void Then_UserService_Create_user_is_called()
        {
            Handler.Handle(new CreatePasswordRequest {InvitationId = InvitationId, Password = "Password"}, CancellationToken.None).Wait();

            UserService.Received().CreateUser(Arg.Is<LoginUser>(u => 
                u.UserName == "email@provider.com" 
                && u.Email == "email@provider.com" 
                && u.GivenName == "GN1" 
                && u.FamilyName == "FN1"), "Password");
        }

        [Test]
        public async Task Then_profile_claims_are_created()
        {
            
            await Handler.Handle(new CreatePasswordRequest {InvitationId = InvitationId, Password = "Password"}, CancellationToken.None);

            await UserService.Received(1).AddUserClaim(Arg.Is<LoginUser>(lu => lu.Id == NewLoginUserId.ToString()), "given_name", "GN1");
            await UserService.Received(1).AddUserClaim(Arg.Is<LoginUser>(lu => lu.Id == NewLoginUserId.ToString()), "family_name", "FN1");
        }
        
        [Test]
        public void Then_Invitation_is_updated_to_IsUserCreated_true()
        {
            Handler.Handle(new CreatePasswordRequest {InvitationId = InvitationId, Password = "Password"}, CancellationToken.None).Wait();

            var invitation = LoginContext.Invitations.Single(i => i.Id == InvitationId);
            invitation.IsUserCreated.Should().BeTrue();
        }

        [Test]
        public void Then_callback_service_is_called()
        {           
            Handler.Handle(new CreatePasswordRequest {InvitationId = InvitationId, Password = "Password"}, CancellationToken.None).Wait();

            CallbackService.Received().Callback(Arg.Is<Invitation>(i => i.SourceId == "ABC123"), NewLoginUserId.ToString());
        }
        
        [Test]
        public void Then_CreatePasswordResponse_PasswordValid_is_true()
        {
            var response = Handler.Handle(new CreatePasswordRequest {InvitationId = InvitationId, Password = "password"}, CancellationToken.None).Result;
            response.PasswordValid.Should().BeTrue();
        }
        
        [Test]
        public async Task Then_LogEntry_created_to_log_invitation()
        {
            SystemTime.UtcNow = () => new DateTime(2019, 1, 1, 1, 1, 1);
            var logId = Guid.NewGuid();
            GuidGenerator.NewGuid = () => logId;

            await Handler.Handle(new CreatePasswordRequest {InvitationId = InvitationId, Password = "password"}, CancellationToken.None);
            
            var logEntry = LoginContext.UserLogs.Single();

            var expectedLogEntry = new UserLog
            {
                Id = logId,
                DateTime = SystemTime.UtcNow(),
                Email = "email@provider.com",
                Action = "Create password",
                Result = "User account created"
            };

            logEntry.Should().BeEquivalentTo(expectedLogEntry);
        }
    }
}