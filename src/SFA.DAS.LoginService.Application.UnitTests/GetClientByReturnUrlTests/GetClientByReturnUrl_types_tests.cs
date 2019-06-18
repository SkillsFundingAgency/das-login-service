using FluentAssertions;
using MediatR;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetClientByReturnUrl;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Types.GetClientByReturnUrl;

namespace SFA.DAS.LoginService.Application.UnitTests.GetClientByReturnUrlTests
{
    [TestFixture]
    public class GetClientByReturnUrl_types_tests
    {
        [Test]
        public void GetClientByLogoutIdRequest_implements_IRequest()
        {
            typeof(GetClientByReturnUrlRequest).Should().Implement<IRequest<Client>>();
        }

        [Test]
        public void GetClientByReturnUrlHandler_implements_IRequestHandler()
        {
            typeof(GetClientByReturnUrlHandler).Should().Implement<IRequestHandler<GetClientByReturnUrlRequest, Client>>();
        }
    }
}