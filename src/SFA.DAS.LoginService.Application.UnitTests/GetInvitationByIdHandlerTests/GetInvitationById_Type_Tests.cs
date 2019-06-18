using FluentAssertions;
using MediatR;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.GetInvitationByIdHandlerTests
{
    [TestFixture]
    public class GetInvitationById_Type_Tests
    {
        [Test]
        public void GetInvitationByIdHandler_Implements_IRequestHandler()
        {
            typeof(GetInvitationByIdHandler).Should()
                .Implement<IRequestHandler<GetInvitationByIdRequest, Invitation>>();
        }
        
        [Test]
        public void GetInvitationByIdRequest_Implements_IRequest()
        {
            typeof(GetInvitationByIdRequest).Should()
                .Implement<IRequest<Invitation>>();
        }
    }
}