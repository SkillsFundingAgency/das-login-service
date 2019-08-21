using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Application.Services.EmailServiceViewModels;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.CreateInvitationHandlerTests
{
    public class When_CreateInvitationHandler_called_With_valid_arguments : CreateInvitationHandlerTestBase
    {
        [Test]
        public void Then_Invitation_is_created_in_database()
        {
            var createInvitationRequest = BuildCreateInvitationRequest();
            CreateInvitationHandler.Handle(createInvitationRequest, CancellationToken.None).Wait();

            LoginContext.Invitations.Count().Should().Be(1);
        }
        
        [Test]
        public void Then_Request_arguments_are_in_Invitation_created_in_database()
        {
            var createInvitationRequest = BuildCreateInvitationRequest();
            CreateInvitationHandler.Handle(createInvitationRequest, CancellationToken.None).Wait();

            var insertedInvitation = LoginContext.Invitations.Single();

            insertedInvitation.Email.Should().Be(createInvitationRequest.Email);
            insertedInvitation.GivenName.Should().Be(createInvitationRequest.GivenName);
            insertedInvitation.FamilyName.Should().Be(createInvitationRequest.FamilyName);
            insertedInvitation.SourceId.Should().Be(createInvitationRequest.SourceId);
            insertedInvitation.CallbackUri.Should().Be(createInvitationRequest.Callback.ToString());
            insertedInvitation.UserRedirectUri.Should().Be(createInvitationRequest.UserRedirect.ToString());
            insertedInvitation.ClientId.Should().Be(createInvitationRequest.ClientId.ToString());
            insertedInvitation.Inviter.Should().Be(createInvitationRequest.Inviter.ToString());
            insertedInvitation.InviterId.Should().Be(createInvitationRequest.InviterId.ToString());
        }

        [Test]
        public void Then_new_Invitation_is_inserted_with_Valid_until_set_one_hour_ahead()
        {
            SystemTime.UtcNow = () => new DateTime(2019,7,9,08,12,43);
            var expectedValidUntilTime = SystemTime.UtcNow().AddHours(1); 
            
            var createInvitationRequest = BuildCreateInvitationRequest();
            CreateInvitationHandler.Handle(createInvitationRequest, CancellationToken.None).Wait();

            var insertedInvitation = LoginContext.Invitations.Single();
           
            insertedInvitation.ValidUntil.Should().Be(expectedValidUntilTime);
        }

        [Test]
        public void Then_an_email_is_sent_out_to_the_invited_user_with_correct_replacement_values()
        {
            var createInvitationRequest = BuildCreateInvitationRequest();
            
            LoginConfig.BaseUrl.Returns("https://goodurl/");
            
            CreateInvitationHandler.Handle(createInvitationRequest, CancellationToken.None).Wait();
            
            var insertedInvitation = LoginContext.Invitations.Single();

            EmailService.Received().SendInvitationEmail(Arg.Is<InvitationEmailViewModel>(vm => 
                vm.Contact == "InvitedGivenName" && 
                vm.ServiceName == "Acme Service" &&
                vm.ServiceTeam == "Acme Service Team" &&
                vm.LoginLink == "https://goodurl/Invitations/CreatePassword/" + insertedInvitation.Id &&
                vm.EmailAddress == createInvitationRequest.Email &&
                vm.TemplateId == InvitationTemplateId));
        }
        
        [Test]
        public void Then_response_invited_is_true()
        {
            var createInvitationRequest = BuildCreateInvitationRequest();
            var response = CreateInvitationHandler.Handle(createInvitationRequest, CancellationToken.None).Result;

            response.Invited.Should().BeTrue();
        }
        
        [Test]
        public void Then_response_invitationId_is_correct()
        {
            var createInvitationRequest = BuildCreateInvitationRequest();
            var response = CreateInvitationHandler.Handle(createInvitationRequest, CancellationToken.None).Result;

            var invitationCreated = LoginContext.Invitations.Single();
            
            response.InvitationId.Should().Be(invitationCreated.Id);
        }

        [Test]
        public async Task Then_LogEntry_created_to_log_invitation()
        {
            SystemTime.UtcNow = () => new DateTime(2019, 1, 1, 1, 1, 1);
            var logId = Guid.NewGuid();
            GuidGenerator.NewGuid = () => logId;

            var createInvitationRequest = BuildCreateInvitationRequest();
            await CreateInvitationHandler.Handle(createInvitationRequest, CancellationToken.None);

            var logEntry = LoginContext.UserLogs.Single();

            var expectedLogEntry = new UserLog
            {
                Id = logId,
                DateTime = SystemTime.UtcNow(),
                Email = "invited@email.com",
                Action = "Invite",
                Result = "Invited"
            };

            logEntry.Should().BeEquivalentTo(expectedLogEntry);
        }
    }
}