using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetClientById;
using SFA.DAS.LoginService.Application.GetClientByLogoutId;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Data.JsonObjects;
using SFA.DAS.LoginService.Types.GetClientById;
using SFA.DAS.LoginService.Types.GetClientByLogoutId;

namespace SFA.DAS.LoginService.Application.UnitTests.GetClientByLogoutIdTests
{
    [TestFixture]
    public class GetClientByLogoutId_called_for_valid_logoutId
    {
        [Test]
        public async Task Then_correct_Client_is_returned()
        {
            var clientId = Guid.NewGuid();
            var logoutId = Convert.ToBase64String(clientId.ToByteArray()); // a fake logoutId which is actually an encrypted string containing the client id
            
            var clientService = Substitute.For<IClientService>();
            clientService.GetByLogoutId(Arg.Is(logoutId), Arg.Any<CancellationToken>()).Returns(new Client()
            {
                Id = clientId,
                ServiceDetails = new ServiceDetails {ServiceName = "Service 2", SupportUrl = "https://support/Url/2"}
            });

            var handler = new GetClientByLogoutIdHandler(clientService);

            var clientResult = await handler.Handle(new GetClientByLogoutIdRequest() {LogoutId = logoutId}, CancellationToken.None);
            clientResult.Id.Should().Be(clientId);
            clientResult.ServiceDetails.ServiceName.Should().Be("Service 2");
            clientResult.ServiceDetails.SupportUrl.Should().Be("https://support/Url/2");
        }
    }
}