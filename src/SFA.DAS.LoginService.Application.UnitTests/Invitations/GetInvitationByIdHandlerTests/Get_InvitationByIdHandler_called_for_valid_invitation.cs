using System;
using System.Threading;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.GetInvitationByIdHandlerTests
{
    [TestFixture]
    public class Get_InvitationByIdHandler_called_for_valid_invitation : GetInvitationByIdHandlerTestBase
    {
        [Test]
        public void Then_Invitation_returned()
        {
            var result = Handler.Handle(new GetInvitationByIdRequest(GoodInvitationId), CancellationToken.None).Result;
            result.Should().BeOfType<Invitation>();
        }

        [Test]
        public void Then_InvitationResponse_returned_with_correct_invitation_data()
        {
            var result = Handler.Handle(new GetInvitationByIdRequest(GoodInvitationId), CancellationToken.None).Result;
            result.Should().BeEquivalentTo(GoodInvitation);
        }
    }
}