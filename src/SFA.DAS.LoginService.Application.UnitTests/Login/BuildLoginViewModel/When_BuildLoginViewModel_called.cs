using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.BuildLoginViewModel;

namespace SFA.DAS.LoginService.Application.UnitTests.Login.BuildLoginViewModel
{
    [TestFixture]
    public class When_BuildLoginViewModel_called
    {
        [Test]
        public async Task Then_LoginViewModel_is_returned()
        {
            var inMemoryClientStore = new InMemoryClientStore(new[] {new Client() {ClientId = "mvc"}});
            var identityServerInteractionService = Substitute.For<IIdentityServerInteractionService>();
            identityServerInteractionService.GetAuthorizationContextAsync("https://returnurl")
                .Returns(new AuthorizationRequest(){ClientId = "mvc"});
            
            var handler = new BuildLoginViewModelHandler(identityServerInteractionService, Substitute.For<IAuthenticationSchemeProvider>(), inMemoryClientStore);
            var result = await handler.Handle(new BuildLoginViewModelRequest() {returnUrl = "https://returnurl"}, CancellationToken.None);
            result.Should().BeOfType<LoginViewModel>();
        }
    }
}