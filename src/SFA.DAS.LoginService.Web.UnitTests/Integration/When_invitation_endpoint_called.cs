using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace SFA.DAS.LoginService.Web.UnitTests.Integration
{
    [TestFixture]
    public class When_invitation_endpoint_called
    {
        [Test]
        public async Task Then_404_is_not_returned()
        {
            var client = new WebApplicationFactory<Startup>().CreateClient();

            var response = await client.PostAsync("/Invitations", new StringContent(""));

            response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task With_no_bearer_token_Then_401_Unauthorized_is_returned()
        {
            var client = new WebApplicationFactory<Startup>().CreateClient();

            var response = await client.PostAsync("/Invitations", new StringContent(""));

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }   
    }
}