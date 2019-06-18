using FluentAssertions;
using MediatR;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetClientByLogoutId;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Types.GetClientByLogoutId;

namespace SFA.DAS.LoginService.Application.UnitTests.GetClientByLogoutIdTests
{
    [TestFixture]
    public class GetClientByLogoutId_types_tests
    {
        [Test]
        public void GetClientByLogoutIdRequest_implements_IRequest()
        {
            typeof(GetClientByLogoutIdRequest).Should().Implement<IRequest<Client>>();
        }

        [Test]
        public void GetClientByLogoutIdHandler_implements_IRequestHandler()
        {
            typeof(GetClientByLogoutIdHandler).Should().Implement<IRequestHandler<GetClientByLogoutIdRequest, Client>>();
        }
    }
}