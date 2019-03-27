using System.Linq;
using System.Threading;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Invitations.CreateInvitation;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Application.Services.EmailServiceViewModels;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.CreateInvitationHandlerTests
{
    public class When_user_already_exists : CreateInvitationHandlerTestBase
    {
        private CreateInvitationRequest _createInvitationRequest;

        [SetUp]
        public void Arrange()
        {
            UserService.FindByEmail("invited@email.com").Returns(new LoginUser());
            _createInvitationRequest = BuildCreateInvitationRequest();
        }
        
        [Test]
        public void Then_user_service_is_checked_for_existing_user()
        {
            CreateInvitationHandler.Handle(_createInvitationRequest, CancellationToken.None).Wait();

            UserService.Received().FindByEmail("invited@email.com");
        }

        [Test]
        public void Then_no_invitation_is_created_or_sent()
        {
            CreateInvitationHandler.Handle(_createInvitationRequest, CancellationToken.None).Wait();

            LoginContext.Invitations.Count().Should().Be(0);
            EmailService.DidNotReceiveWithAnyArgs()
                .SendInvitationEmail(Arg.Any<InvitationEmailViewModel>());
        }
        
        [Test]
        public void Then_response_states_that_user_already_exists()
        {
            var response = CreateInvitationHandler.Handle(_createInvitationRequest, CancellationToken.None).Result;

            response.Message.Should().Be("User already exists");
            response.Invited.Should().Be(false);
        }
    }
}