using System;
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
        private string _userId;

        [SetUp]
        public void Arrange()
        {
            _userId = Guid.NewGuid().ToString();
            UserService.FindByEmail("invited@email.com").Returns(new LoginUser(){Id = _userId});
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
            response.ExistingUserId.Should().Be(_userId);
        }

        [Test]
        public void Then_user_already_exists_email_is_sent()
        {
            CreateInvitationHandler.Handle(_createInvitationRequest, CancellationToken.None).Wait();
            EmailService.Received().SendUserExistsEmail(Arg.Is<UserExistsEmailViewModel>(
                vm => 
                    vm.ServiceName == "Acme Service" 
                    && vm.ServiceTeam == "Acme Service Team"
                    && vm.Contact == _createInvitationRequest.GivenName
                    && vm.LoginLink == "https://serviceurl"
                    && vm.EmailAddress == _createInvitationRequest.Email));
        }
    }
}