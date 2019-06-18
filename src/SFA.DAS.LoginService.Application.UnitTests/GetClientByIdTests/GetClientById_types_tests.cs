using FluentAssertions;
using MediatR;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetClientById;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Types.GetClientById;

namespace SFA.DAS.LoginService.Application.UnitTests.GetClientByIdTests
{
    [TestFixture]
    public class GetClientById_types_tests
    {
        [Test]
        public void GetClientByIdRequest_implements_IRequest()
        {
            typeof(GetClientByIdRequest).Should().Implement<IRequest<Client>>();
        }

        [Test]
        public void GetClientByIdHandler_implements_IRequestHandler()
        {
            typeof(GetClientByIdHandler).Should().Implement<IRequestHandler<GetClientByIdRequest, Client>>();
        }
    }
}