using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetClientById;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Data.JsonObjects;
using SFA.DAS.LoginService.Types.GetClientById;

namespace SFA.DAS.LoginService.Application.UnitTests.GetClientByIdTests
{
    [TestFixture]
    public class GetClientById_called_for_valid_clientId
    {
        [Test]
        public async Task Then_correct_Client_is_returned()
        {
            var clientId = Guid.NewGuid();

            var clientService = Substitute.For<IClientService>();
            clientService.GetByClientId(Arg.Is(clientId), Arg.Any<CancellationToken>()).Returns(new Client()
            {
                Id = clientId,
                ServiceDetails = new ServiceDetails {ServiceName = "Service 2", SupportUrl = "https://support/Url/2"}
            });

            var handler = new GetClientByIdHandler(clientService);

            var clientResult = await handler.Handle(new GetClientByIdRequest() {ClientId = clientId}, CancellationToken.None);
            clientResult.Id.Should().Be(clientId);
            clientResult.ServiceDetails.ServiceName.Should().Be("Service 2");
            clientResult.ServiceDetails.SupportUrl.Should().Be("https://support/Url/2");
        }
    }
}