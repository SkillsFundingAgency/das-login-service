using System.Linq;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.CreateInvitationHandlerTests
{
    public class When_user_already_exists : CreateInvitationHandlerTestBase
    {
        [Test]
        public void Then_user_service_is_checked_for_existing_user()
        {
            var createInvitationRequest = BuildCreateInvitationRequest();
            CreateInvitationHandler.Handle(createInvitationRequest, CancellationToken.None).Wait();

            UserService.Received().UserExists("invited@email.com");
        }

        [Test]
        public void Then_no_invitation_is_created_or_sent()
        {
            UserService.UserExists("invited@email.com").Returns(true);
            
            var createInvitationRequest = BuildCreateInvitationRequest();
            CreateInvitationHandler.Handle(createInvitationRequest, CancellationToken.None).Wait();

            LoginContext.Invitations.Count().Should().Be(0);
            EmailService.DidNotReceiveWithAnyArgs()
                .SendInvitationEmail("invited@email.com", Arg.Any<string>(), Arg.Any<string>());
        }
        
        [Test]
        public void Then_response_states_that_user_already_exists()
        {
            UserService.UserExists("invited@email.com").Returns(true);
            
            var createInvitationRequest = BuildCreateInvitationRequest();
            var response = CreateInvitationHandler.Handle(createInvitationRequest, CancellationToken.None).Result;

            response.Message.Should().Be("User already exists");
            response.Invited.Should().Be(false);
        }
    }
}