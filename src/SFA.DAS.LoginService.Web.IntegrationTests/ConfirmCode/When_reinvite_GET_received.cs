using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace SFA.DAS.LoginService.Web.IntegrationTests.ConfirmCode
{
    [TestFixture]
    public class When_reinvite_GET_received
    {
        [Test]
        public async Task Then_404_NotFound_is_not_returned()
        {
            var client = new WebApplicationFactory<Startup>().CreateClient();

            var response = await client.GetAsync("/Invitations/Reinvite/" + Guid.NewGuid());

            response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
        }
    }
}