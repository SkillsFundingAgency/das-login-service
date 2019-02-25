using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace SFA.DAS.LoginService.Web.UnitTests.Integration.ConfirmCode
{
    [TestFixture]
    public class When_confirm_code_endpoint_GET
    {
        [Test]
        public async Task The_404_NotFound_is_not_returned()
        {
            var client = new WebApplicationFactory<Startup>().CreateClient();

            var response = await client.GetAsync("/Invitations/ConfirmCode/" + Guid.NewGuid());

            response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
        }
    }
}