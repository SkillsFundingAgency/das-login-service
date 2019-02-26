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
    }
}