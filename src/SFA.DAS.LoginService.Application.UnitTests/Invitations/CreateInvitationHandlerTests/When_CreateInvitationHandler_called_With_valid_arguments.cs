using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Services;

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
        }

        [Test]
        public void Then_code_generation_service_is_asked_for_a_code()
        {
            var createInvitationRequest = BuildCreateInvitationRequest();
            CreateInvitationHandler.Handle(createInvitationRequest, CancellationToken.None).Wait();

            CodeGenerationService.Received().GenerateCode();
        }

        [Test]
        public void Then_hashing_service_is_asked_to_hash_the_code()
        {
            var createInvitationRequest = BuildCreateInvitationRequest();
            
            CodeGenerationService.GenerateCode().Returns("ABC123XY");
            
            CreateInvitationHandler.Handle(createInvitationRequest, CancellationToken.None).Wait();

            HashingService.Received().GetHash("ABC123XY");
        }

        [Test]
        public void Then_new_Invitation_is_inserted_with_hashed_code()
        {
            var createInvitationRequest = BuildCreateInvitationRequest();

            HashingService.GetHash(Arg.Any<string>()).Returns("ThisIsTheHash=");
            
            CreateInvitationHandler.Handle(createInvitationRequest, CancellationToken.None).Wait();

            var insertedInvitation = LoginContext.Invitations.Single();
            
            insertedInvitation.Code.Should().Be("ThisIsTheHash=");
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
            
            CodeGenerationService.GenerateCode().Returns("ABC123XY");
            LoginConfig.BaseUrl.Returns("https://goodurl/");
            
            CreateInvitationHandler.Handle(createInvitationRequest, CancellationToken.None).Wait();
            
            var insertedInvitation = LoginContext.Invitations.Single();

            EmailService.Received().SendInvitationEmail(createInvitationRequest.Email, "ABC123XY",
                "https://goodurl/Invitation/ConfirmCode/" + insertedInvitation.Id);
        }
    }
}