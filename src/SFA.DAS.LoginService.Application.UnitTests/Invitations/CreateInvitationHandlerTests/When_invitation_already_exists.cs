using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Invitations.CreateInvitation;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.CreateInvitationHandlerTests
{
    public class When_invitation_already_exists : CreateInvitationHandlerTestBase
    {
        [SetUp]
        public void Arrange()
        {
            _existingInvitationId = Guid.NewGuid();
            LoginContext.Invitations.Add(new Invitation()
                {Email = "invited@email.com", Id = _existingInvitationId});
            LoginContext.Invitations.Add(new Invitation() {Email = "someother@email.com", Id = Guid.NewGuid()});
            LoginContext.SaveChanges();
            
            _createInvitationRequest = BuildCreateInvitationRequest();
        }

        private Guid _existingInvitationId;
        private CreateInvitationRequest _createInvitationRequest;

        [Test]
        public void Then_existing_record_is_deleted()
        {
            CreateInvitationHandler.Handle(_createInvitationRequest, CancellationToken.None).Wait();

            LoginContext.Invitations.Count(i => i.Email == "invited@email.com" && i.Id == _existingInvitationId).Should().Be(0);
            LoginContext.Invitations.Count(i => i.Email == "invited@email.com" && i.Id != _existingInvitationId).Should().Be(1);
            LoginContext.Invitations.Count(i => i.Email != "invited@email.com").Should().Be(1);
        }

        [Test]
        public void Then_new_id_and_code_do_not_match_existing()
        {
            CreateInvitationHandler.Handle(_createInvitationRequest, CancellationToken.None).Wait();

            var newInvitation = LoginContext.Invitations.Single(i => i.Email == "invited@email.com");

            newInvitation.Id.Should().NotBe(_existingInvitationId);
        }

        [Test]
        public void Then_new_new_invite_is_created()
        {
            var response = CreateInvitationHandler.Handle(_createInvitationRequest, CancellationToken.None).Result;
            response.Invited.Should().BeTrue();
        }
    }
}