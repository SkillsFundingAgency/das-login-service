using System;
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
    public class BuildLoginViewModelTestBase
    {
        protected BuildLoginViewModelHandler Handler;
        protected LoginContext LoginContext;
        protected Guid ClientId;

        [SetUp]
        public void SetUp()
        {
            var inMemoryClientStore = new InMemoryClientStore(new[] {new Client() {ClientId = "mvc"}});

            var identityServerInteractionService = Substitute.For<IIdentityServerInteractionService>();
            identityServerInteractionService.GetAuthorizationContextAsync("https://returnurl")
                .Returns(new AuthorizationRequest(){ClientId = "mvc"});
            
            var dbContextOptions = new DbContextOptionsBuilder<LoginContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            LoginContext = new LoginContext(dbContextOptions);

            ClientId = Guid.NewGuid();
            
            Handler = new BuildLoginViewModelHandler(identityServerInteractionService, Substitute.For<IAuthenticationSchemeProvider>(), inMemoryClientStore, LoginContext);
        }
    }
}