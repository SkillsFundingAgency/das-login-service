using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace SFA.DAS.LoginService.Web.IntegrationTests.Login
{
    [TestFixture]
    public class When_Login_POST_called
    {
        [Test]
        public async Task Then_404_NotFound_Is_Not_Returned()
        {
            var client = new WebApplicationFactory<Startup>().CreateClient();

            var response = await client.PostAsync("/Account/Login", new StringContent(""));

            response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
            response.StatusCode.Should().NotBe(HttpStatusCode.MethodNotAllowed);
        }
    }
}