using FluentAssertions;
using MediatR;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ProcessLogin;

namespace SFA.DAS.LoginService.Application.UnitTests.Login.ProcessLogin
{
    [TestFixture]
    public class ProcessLogin_Types_Tests
    {
        [Test]
        public void ProcessLoginRequest_should_implement_IRequest()
        {
            typeof(ProcessLoginRequest).Should().Implement<IRequest<ProcessLoginResponse>>();
        }

        [Test]
        public void ProcessLoginHandler_should_implement_IRequestHandler()
        {
            typeof(ProcessLoginHandler).Should().Implement<IRequestHandler<ProcessLoginRequest, ProcessLoginResponse>>();
        }
    }
}