using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.JsonObjects;
using Client = SFA.DAS.LoginService.Data.Entities.Client;

namespace SFA.DAS.LoginService.Application.UnitTests.Services.ClientServiceTests
{
    [TestFixture]
    public class GetClientByLogoutId_called_for_valid_logoutId
    {
        [Test]
        public async Task Then_correct_Client_is_returned()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LoginContext>()
                .UseInMemoryDatabase(databaseName: "GetClientByLogoutIdHandler_tests")
                .Options;

            var loginContext = new LoginContext(dbContextOptions);

            var clientId = Guid.NewGuid();
            var identitySeverClientId = Guid.NewGuid();
            var logoutId = Convert.ToBase64String(identitySeverClientId.ToByteArray()); // a fake returnUrl which is actually an encrypted string containing the client id

            loginContext.Clients.AddRange(new List<Client>
            {
                new Client(){Id = Guid.NewGuid(), IdentityServerClientId = Guid.NewGuid().ToString(), ServiceDetails = new ServiceDetails{ServiceName = "Service 1", SupportUrl = "https://support/Url/1"}},
                new Client(){Id = clientId, IdentityServerClientId = identitySeverClientId.ToString(), ServiceDetails = new ServiceDetails{ServiceName = "Service 2", SupportUrl = "https://support/Url/2"}},
                new Client(){Id = Guid.NewGuid(), IdentityServerClientId = Guid.NewGuid().ToString(), ServiceDetails = new ServiceDetails{ServiceName = "Service 3", SupportUrl = "https://support/Url/3"}},
            });
            await loginContext.SaveChangesAsync();

            var interactionService = Substitute.For<IIdentityServerInteractionService>();
            interactionService.GetLogoutContextAsync(Arg.Is(logoutId)).Returns(new LogoutRequest(String.Empty, new LogoutMessage(new ValidatedEndSessionRequest()))
            {
                ClientId = identitySeverClientId.ToString()
            });
               
            var clientService = new ClientService(interactionService, loginContext);

            var clientResult = await clientService.GetByLogoutId(logoutId, CancellationToken.None);

            clientResult.Id.Should().Be(clientId);
            clientResult.ServiceDetails.ServiceName.Should().Be("Service 2");
            clientResult.ServiceDetails.SupportUrl.Should().Be("https://support/Url/2");
        }
    }
}