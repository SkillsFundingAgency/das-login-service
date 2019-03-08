using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace SFA.DAS.LoginService.Web.IntegrationTests.ConfirmCode
{
    public class When_reinvite_POST_received
    {
        [Test]
        public async Task Then_404_NotFound_Is_Not_Returned()
        {
            var client = new WebApplicationFactory<Startup>().CreateClient();

            var response = await client.PostAsync("/Invitations/Reinvite/" + Guid.NewGuid(), new StringContent(""));

            response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
            response.StatusCode.Should().NotBe(HttpStatusCode.MethodNotAllowed);
        }
    }
}