using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetClientById;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.GetClientByIdTests
{
    [TestFixture]
    public class GetClientById_called_for_valid_clientId
    {
        [Test]
        public async Task Then_correct_Client_is_returned()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LoginContext>()
                .UseInMemoryDatabase(databaseName: "GetClientByIdHandler_tests")
                .Options;

            var loginContext = new LoginContext(dbContextOptions);

            var clientId = Guid.NewGuid();
            loginContext.Clients.AddRange(new List<Client>
            {
                new Client(){Id = Guid.NewGuid(), ServiceName = "Service 1"},
                new Client(){Id = clientId, ServiceName = "Service 2"},
                new Client(){Id = Guid.NewGuid(), ServiceName = "Service 3"},
            });
            await loginContext.SaveChangesAsync();

            var handler = new GetClientByIdHandler(loginContext);

            var clientResult = await handler.Handle(new GetClientByIdRequest() {ClientId = clientId}, CancellationToken.None);

            clientResult.Id.Should().Be(clientId);
            clientResult.ServiceName.Should().Be("Service 2");
        }
    }
}