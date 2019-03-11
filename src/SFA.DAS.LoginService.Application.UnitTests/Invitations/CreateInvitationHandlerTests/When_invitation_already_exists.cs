using System;
using System.Linq;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.CreateInvitationHandlerTests
{
    public class When_invitation_already_exists : CreateInvitationHandlerTestBase
    {
        private void SetupExistingInvitation()
        {
            _existingInvitationId = Guid.NewGuid();
            LoginContext.Invitations.Add(new Invitation()
                {Email = "invited@email.com", Id = _existingInvitationId});
            LoginContext.Invitations.Add(new Invitation() {Email = "someother@email.com", Id = Guid.NewGuid()});
            LoginContext.SaveChanges();
        }

        private Guid _existingInvitationId;

        [Test]
        public void Then_existing_record_is_deleted()
        {
           SetupExistingInvitation();

            var createInvitationRequest = BuildCreateInvitationRequest();
            CreateInvitationHandler.Handle(createInvitationRequest, CancellationToken.None).Wait();

            LoginContext.Invitations.Count(i => i.Email == "invited@email.com" && i.Id == _existingInvitationId).Should().Be(0);
            LoginContext.Invitations.Count(i => i.Email == "invited@email.com" && i.Id != _existingInvitationId).Should().Be(1);
            LoginContext.Invitations.Count(i => i.Email != "invited@email.com").Should().Be(1);
        }

        [Test]
        public void Then_new_id_and_code_do_not_match_existing()
        {
            SetupExistingInvitation();

            var createInvitationRequest = BuildCreateInvitationRequest();
            CreateInvitationHandler.Handle(createInvitationRequest, CancellationToken.None).Wait();

            var newInvitation = LoginContext.Invitations.Single(i => i.Email == "invited@email.com");

            newInvitation.Id.Should().NotBe(_existingInvitationId);
        }

        [Test]
        public void Then_new_new_invite_is_created()
        {
            SetupExistingInvitation();
            var createInvitationRequest = BuildCreateInvitationRequest();
            var response = CreateInvitationHandler.Handle(createInvitationRequest, CancellationToken.None).Result;
            response.Invited.Should().BeTrue();
        }
    }
}