using FluentAssertions;
using MediatR;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ConfirmCode;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.ConfirmCodeTests
{
    [TestFixture]
    public class ConfirmCode_Type_Tests
    {
        [Test]
        public void ConfirmCodeRequest_implements_IRequest()
        {
            typeof(ConfirmCodeViewModel).Should().Implement<IRequest<ConfirmCodeResponse>>();
        }

        [Test]
        public void ConfirmCodeHandler_implements_IRequestHandler()
        {
            typeof(ConfirmCodeHandler).Should().Implement<IRequestHandler<ConfirmCodeViewModel, ConfirmCodeResponse>>();
        }
    }
}