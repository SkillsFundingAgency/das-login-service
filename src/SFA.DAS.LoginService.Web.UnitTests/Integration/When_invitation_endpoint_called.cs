using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        [Test]
        public async Task With_valid_bearer_token_Then_401_is_not_returned()
        {
            var client = new CustomWebApplicationFactory<Startup>().CreateClient();

            client.SetBearerToken(GetBearerToken(client));

            var response = await client.PostAsync("/Invitations", new StringContent(""));

            response.StatusCode.Should().NotBe(HttpStatusCode.Unauthorized);
        }

        private string GetBearerToken(HttpClient client)
        {
            var disco = client.GetDiscoveryDocumentAsync("http://localhost").Result;

            // request token
            var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A",
                Scope = "api1"
            }).Result;

            return tokenResponse.AccessToken;
        }
    }

    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
                services.AddAuthentication()
                    .AddJwtBearer(jwt =>
                    {
                        jwt.Authority = "http://localhost";
                        jwt.RequireHttpsMetadata = false;
                        jwt.Audience = "api1";
                    });
            
                services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddInMemoryClients(Config.GetClients())
                    .AddInMemoryApiResources(Config.GetApis())
                    .AddInMemoryIdentityResources(Config.GetIdentityResources());
            });
        }
    }
}