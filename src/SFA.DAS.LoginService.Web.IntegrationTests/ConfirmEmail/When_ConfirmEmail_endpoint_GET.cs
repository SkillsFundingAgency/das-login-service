using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace SFA.DAS.LoginService.Web.IntegrationTests.ConfirmEmail
{
    [TestFixture]
    public class When_ConfirmEmail_endpoint_GET
    {
        [Test]
        public async Task Then_404_NotFound_is_not_returned()
        {
            var client = new WebApplicationFactory<Startup>().CreateClient();

            // currently unable to pass actual escaped strings in the request as the WebApplicationFactory unescapes them
            // before making the request and this is then an invalid request
            string returnUrl = Uri.EscapeDataString("returnurlstring");
            string identityToken = Uri.EscapeDataString("tokenstring");

            var requestUrl = $"/ConfirmEmail/{returnUrl}/{identityToken}";
            var response = await client.GetAsync(requestUrl);

            response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
        }
    }
}