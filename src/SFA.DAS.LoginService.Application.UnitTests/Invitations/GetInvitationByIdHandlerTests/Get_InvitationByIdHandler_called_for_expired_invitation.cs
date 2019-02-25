using System;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetInvitationById;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.GetInvitationByIdHandlerTests
{
    [TestFixture]
    public class Get_InvitationByIdHandler_called_for_expired_invitation : GetInvitationByIdHandlerTestBase
    {
        [Test]
        public void Then_null_is_returned()
        {
            var result = Handler.Handle(new GetInvitationByIdRequest(ExpiredInvitationId), CancellationToken.None).Result;
            result.Should().BeNull();
        }
    }

    [TestFixture]
    public class Get_InvitationIdByHandler_called_for_nonexistant_invitation : GetInvitationByIdHandlerTestBase
    {
        [Test]
        public void Then_null_is_returned()
        {
            var result = Handler.Handle(new GetInvitationByIdRequest(Guid.NewGuid()), CancellationToken.None).Result;
            result.Should().BeNull();
        }
    }
}