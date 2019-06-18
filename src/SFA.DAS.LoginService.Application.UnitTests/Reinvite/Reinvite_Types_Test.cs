using FluentAssertions;
using MediatR;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Invitations.CreateInvitation;
using SFA.DAS.LoginService.Application.Reinvite;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.Reinvite
{
    [TestFixture]
    public class Reinvite_Types_Test
    {
        [Test]
        public void ReinviteRequest_implements_IRequest()
        {
            typeof(ReinviteRequest).Should().Implement<IRequest<CreateInvitationResponse>>();
        }

        [Test]
        public void ReinviteHandler_implements_IRequestHandler()
        {
            typeof(ReinviteHandler).Should().Implement<IRequestHandler<ReinviteRequest, CreateInvitationResponse>>();
        }
    }
}