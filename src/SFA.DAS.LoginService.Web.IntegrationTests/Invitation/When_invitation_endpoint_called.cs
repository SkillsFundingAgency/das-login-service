using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace SFA.DAS.LoginService.Web.IntegrationTests.Invitation
{
    [TestFixture]
    public class When_invitation_endpoint_called
    {
        [Test]
        public async Task Then_404_is_not_returned()
        {
            var client = new WebApplicationFactory<Startup>().CreateClient();

            var response = await client.PostAsync("/Invitations/" + Guid.NewGuid(), new StringContent(""));

            response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
        }
    }
}