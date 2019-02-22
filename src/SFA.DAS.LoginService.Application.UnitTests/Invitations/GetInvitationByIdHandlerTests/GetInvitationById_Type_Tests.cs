using FluentAssertions;
using MediatR;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetInvitationById;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.GetInvitationByIdHandlerTests
{
    [TestFixture]
    public class GetInvitationById_Type_Tests
    {
        [Test]
        public void GetInvitationByIdHandler_Implements_IRequestHandler()
        {
            typeof(GetInvitationByIdHandler).Should()
                .Implement<IRequestHandler<GetInvitationByIdRequest, InvitationResponse>>();
        }
        
        [Test]
        public void GetInvitationByIdRequest_Implements_IRequest()
        {
            typeof(GetInvitationByIdRequest).Should()
                .Implement<IRequest<InvitationResponse>>();
        }
    }
}