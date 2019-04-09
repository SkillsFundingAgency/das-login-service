using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace SFA.DAS.LoginService.Web.IntegrationTests.Login
{
    [TestFixture]
    public class When_connect_authorize_called
    {
        [Test]
        public async Task Then_404_is_not_received()
        {
            var client = new WebApplicationFactory<Startup>().CreateClient(new WebApplicationFactoryClientOptions()
            { AllowAutoRedirect = false });

            var response = await client.GetAsync("/connect/authorize?client_id=apply&redirect_uri=https%3A%2F%2Flocalhost%3A6016%2Fsignin-oidc&response_type=id_token&scope=openid&response_mode=form_post&nonce=636870306741009302.OGQ0NzNiNGMtYTI0NC00MWYyLTkwN2EtYTZjYWYzMWEzZDlmYjk3NzYzOGEtMTQyZC00NmVjLTkxNzEtMzcyMzI2MDY0OGRl&state=CfDJ8LFoUd-yaHtCvaoVlhv1Wfqc67BTxjkNx_lTq5KspA_63Dunb1MzY5ns9cm0Q6EzZNRESvUudLno7ONZKTmCrPHfC9RxRuUOUtH9zhLWvwNI8T7jB3oljq4cZK0JVTxvBP1Tjd2cuGrEH1i1sDsbxVPKBSktOVv2H3pow-OPVT-DEOtN2pCGG8IeyqgM_QEpaTdNDIaBHJ0ZdfDL2CQ8VmMAowJxtA2IbEcJ7nIAC_1kmodGjaumlQl8EaDWolr6m-D5E9OAtgM-pby1SnZKXR4lnBp65fc3uLY8TtrLsVpz_lSyb2Gw7oxuNUI4hyIbbw&x-client-SKU=ID_NETSTANDARD1_4&x-client-ver=5.2.0.0");

            response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Then_302_Redirect_is_received_with_correct_redirect_location()
        {
            var client = new WebApplicationFactory<Startup>().CreateClient(new WebApplicationFactoryClientOptions()
            { AllowAutoRedirect = false });

            var response = await client.GetAsync("/connect/authorize?client_id=apply&redirect_uri=https%3A%2F%2Flocalhost%3A6016%2Fsignin-oidc&response_type=id_token&scope=openid&response_mode=form_post&nonce=636870306741009302.OGQ0NzNiNGMtYTI0NC00MWYyLTkwN2EtYTZjYWYzMWEzZDlmYjk3NzYzOGEtMTQyZC00NmVjLTkxNzEtMzcyMzI2MDY0OGRl&state=CfDJ8LFoUd-yaHtCvaoVlhv1Wfqc67BTxjkNx_lTq5KspA_63Dunb1MzY5ns9cm0Q6EzZNRESvUudLno7ONZKTmCrPHfC9RxRuUOUtH9zhLWvwNI8T7jB3oljq4cZK0JVTxvBP1Tjd2cuGrEH1i1sDsbxVPKBSktOVv2H3pow-OPVT-DEOtN2pCGG8IeyqgM_QEpaTdNDIaBHJ0ZdfDL2CQ8VmMAowJxtA2IbEcJ7nIAC_1kmodGjaumlQl8EaDWolr6m-D5E9OAtgM-pby1SnZKXR4lnBp65fc3uLY8TtrLsVpz_lSyb2Gw7oxuNUI4hyIbbw&x-client-SKU=ID_NETSTANDARD1_4&x-client-ver=5.2.0.0");

            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            response.Headers.Location.ToString().Should()
                .Match(s => s.Contains("/Account/Login"));
        }
    }
}