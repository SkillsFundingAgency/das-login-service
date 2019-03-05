using FluentAssertions;
using MediatR;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.BuildLogoutViewModel;

namespace SFA.DAS.LoginService.Application.UnitTests.Logout
{
    [TestFixture]
    public class Logout_Types_Tests
    {
        [Test]
        public void BuildLogoutViewModelRequest_should_implement_IRequest()
        {
            typeof(LogoutRequest).Should().Implement<IRequest<LogoutResponse>>();
        }

        [Test]
        public void BuildLogoutViewModelHandler_should_implement_IRequestHandler()
        {
            typeof(LogoutHandler).Should().Implement<IRequestHandler<LogoutRequest, LogoutResponse>>();
        }
    }

    
}