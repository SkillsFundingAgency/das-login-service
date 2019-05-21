using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetClientById;
using SFA.DAS.LoginService.Application.GetClientByLogoutId;
using SFA.DAS.LoginService.Application.GetClientByReturnUrl;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Data.JsonObjects;
using SFA.DAS.LoginService.Types.GetClientById;
using SFA.DAS.LoginService.Types.GetClientByLogoutId;
using SFA.DAS.LoginService.Types.GetClientByReturnUrl;

namespace SFA.DAS.LoginService.Application.UnitTests.GetClientByReturnUrlTests
{
    [TestFixture]
    public class GetClientByReturnUrl_called_for_valid_returnUrl
    {
        [Test]
        public async Task Then_correct_Client_is_returned()
        {
            var clientId = Guid.NewGuid();
            var returnUrl = Convert.ToBase64String(clientId.ToByteArray()); // a fake returnUrl which is actually an encrypted string containing the client id
            
            var clientService = Substitute.For<IClientService>();
            clientService.GetByReturnUrl(Arg.Is(returnUrl), Arg.Any<CancellationToken>()).Returns(new Client()
            {
                Id = clientId,
                ServiceDetails = new ServiceDetails {ServiceName = "Service 2", SupportUrl = "https://support/Url/2"}
            });

            var handler = new GetClientByReturnUrlHandler(clientService);

            var clientResult = await handler.Handle(new GetClientByReturnUrlRequest() {ReturnUrl = returnUrl}, CancellationToken.None);
            clientResult.Id.Should().Be(clientId);
            clientResult.ServiceDetails.ServiceName.Should().Be("Service 2");
            clientResult.ServiceDetails.SupportUrl.Should().Be("https://support/Url/2");
        }
    }
}