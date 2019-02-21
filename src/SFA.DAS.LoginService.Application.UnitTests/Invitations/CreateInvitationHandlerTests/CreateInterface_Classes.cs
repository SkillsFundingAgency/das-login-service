using FluentAssertions;
using MediatR;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Invitations.CreateInvitation;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.CreateInvitationHandlerTests
{
    [TestFixture]
    public class CreateInterface_Classes
    {
        [Test]
        public void CreateInvitationRequest_should_implement_IRequest_with_Type_of_CreateInvitationResponse()
        {
            typeof(CreateInvitationRequest).Should().Implement<IRequest<CreateInvitationResponse>>();
        }

        [Test]
        public void CreateInvitationHandler_should_implement_IRequestHandler_with_Type_of_CreateInvitationRequest()
        {
            typeof(CreateInvitationHandler).Should().Implement<IRequestHandler<CreateInvitationRequest, CreateInvitationResponse>>();
        }
    }
}