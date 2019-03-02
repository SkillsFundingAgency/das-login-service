using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.BuildLoginViewModel;
using SFA.DAS.LoginService.Data;

namespace SFA.DAS.LoginService.Application.UnitTests.Login.BuildLoginViewModel
{
    [TestFixture]
    public class When_BuildLoginViewModel_called
    {
        private BuildLoginViewModelHandler _handler;
        private LoginContext _loginContext;

        [SetUp]
        public void SetUp()
        {
            var inMemoryClientStore = new InMemoryClientStore(new[] {new Client() {ClientId = "mvc"}});
            var identityServerInteractionService = Substitute.For<IIdentityServerInteractionService>();
            identityServerInteractionService.GetAuthorizationContextAsync("https://returnurl")
                .Returns(new AuthorizationRequest(){ClientId = "mvc"});
            
            var dbContextOptions = new DbContextOptionsBuilder<LoginContext>()
                .UseInMemoryDatabase(databaseName: "BuildLoginViewModelHandler_tests")
                .Options;

            _loginContext = new LoginContext(dbContextOptions);

            _loginContext.Clients.Add(new Data.Entities.Client(){IdentityServerClientId = "mvc", ServiceName = "Acme Service"});
            _loginContext.SaveChanges();
            
            _handler = new BuildLoginViewModelHandler(identityServerInteractionService, Substitute.For<IAuthenticationSchemeProvider>(), inMemoryClientStore, _loginContext);
        }
        
        [Test]
        public async Task Then_LoginViewModel_is_returned()
        {
            var result = await _handler.Handle(new BuildLoginViewModelRequest() {returnUrl = "https://returnurl"}, CancellationToken.None);
            result.Should().BeOfType<LoginViewModel>();
            result.ServiceName.Should().Be("Acme Service");
            result.ReturnUrl.Should().Be("https://returnurl");
        }
    }
}