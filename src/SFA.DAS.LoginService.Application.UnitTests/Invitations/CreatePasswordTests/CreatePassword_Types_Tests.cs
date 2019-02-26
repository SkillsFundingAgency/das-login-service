using FluentAssertions;
using MediatR;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.CreatePassword;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.CreatePasswordTests
{
    [TestFixture]
    public class CreatePassword_Types_Tests
    {
        [Test]
        public void CreatePasswordRequest_should_implement_IRequest()
        {
            typeof(CreatePasswordRequest).Should().Implement<IRequest<CreatePasswordResponse>>();
        }

        [Test]
        public void CreatePasswordHandler_should_implement_IRequestHandler()
        {
            typeof(CreatePasswordHandler).Should().Implement<IRequestHandler<CreatePasswordRequest, CreatePasswordResponse>>();
        }
    }
}