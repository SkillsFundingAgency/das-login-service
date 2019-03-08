using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace SFA.DAS.LoginService.Web.IntegrationTests.CreatePassword
{
    [TestFixture]
    public class When_CreatePassword_endpoint_GET
    {
        [Test]
        public async Task Then_404_NotFound_is_not_returned()
        {
            var client = new WebApplicationFactory<Startup>().CreateClient();

            var response = await client.GetAsync("/Invitations/CreatePassword/" + Guid.NewGuid());

            response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
        }
    }
}