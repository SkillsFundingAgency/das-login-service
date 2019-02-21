using NUnit.Framework;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.CreateInvitationHandlerTests
{
    
    public class When_invitation_already_exists : CreateInvitationHandlerTestBase
    {
        [Test]
        public void Then_database_is_checked_for_existing_invite()
        {
            Assert.Fail();
        }
        
        [Test]
        public void Then_existing_record_is_deleted()
        {
            Assert.Fail();
        }
        
        [Test]
        public void Then_new_id_and_code_do_not_match_existing()
        {
            Assert.Fail();
        }
        
        [Test]
        public void Then_new_new_invite_is_created()
        {
            Assert.Fail();
        }
    }

    public class When_user_already_exists : CreateInvitationHandlerTestBase
    {
        [Test]
        public void Then_user_service_is_checked_for_existing_user()
        {
            Assert.Fail();
        }

        [Test]
        public void Then_invite_is_not_created()
        {
            Assert.Fail();
        }

        [Test]
        public void Then_email_is_not_sent()
        {
            Assert.Fail();
        }
    }
}