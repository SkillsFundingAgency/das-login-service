using System;
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
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Data.JsonObjects;
using Client = IdentityServer4.Models.Client;

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

    public class When_BuildLoginViewModel_called_for_client_with_allowLocalSignIn_false_and_CreateAccountLinkSupplied : BuildLoginViewModelTestBase
    {
        [SetUp]
        public async Task Arrange()
        {
            LoginContext.Clients.Add(new Data.Entities.Client()
            {
                IdentityServerClientId = "mvc", 
                ServiceDetails  = new ServiceDetails{ServiceName = "Acme Service", SupportUrl = "https://acme.gov.uk/Support", CreateAccountUrl = "https://acme.gov.uk/CreateAccount"},
                Id = ClientId,
                AllowLocalSignUp = false
            });
            await LoginContext.SaveChangesAsync();
        }

        [Test]
        public async Task Then_LoginViewModel_contains_correct_CreateAccount_details()
        {
            var result = await Handler.Handle(new BuildLoginViewModelRequest{returnUrl = "https://returnurl"}, CancellationToken.None);
            result.CreateAccount.LocalSignUp.Should().BeFalse();
            result.CreateAccount.CreateAccountUrl.Should().Be("https://acme.gov.uk/CreateAccount");
        }
    }
    
    public class When_BuildLoginViewModel_called_for_client_with_allowLocalSignIn_true : BuildLoginViewModelTestBase
    {
        [SetUp]
        public async Task Arrange()
        {
            LoginContext.Clients.Add(new Data.Entities.Client()
            {
                IdentityServerClientId = "mvc", 
                ServiceDetails  = new ServiceDetails{ServiceName = "Acme Service", SupportUrl = "https://acme.gov.uk/Support"},
                Id = ClientId,
                AllowLocalSignUp = true
            });
            await LoginContext.SaveChangesAsync();
        }

        [Test]
        public async Task Then_LoginViewModel_contains_correct_CreateAccount_details()
        {
            var result = await Handler.Handle(new BuildLoginViewModelRequest{returnUrl = "https://returnurl"}, CancellationToken.None);
            result.CreateAccount.LocalSignUp.Should().BeTrue();
        }
    }
    
    
    public class When_BuildLoginViewModel_called : BuildLoginViewModelTestBase
    {
        [SetUp]
        public async Task Arrange()
        {
            LoginContext.Clients.Add(new Data.Entities.Client()
            {
                IdentityServerClientId = "mvc", 
                ServiceDetails  = new ServiceDetails{ServiceName = "Acme Service", SupportUrl = "https://acme.gov.uk/Support"},
                Id = ClientId
            });
            await LoginContext.SaveChangesAsync();
        }
        
        [Test]
        public async Task Then_LoginViewModel_is_returned()
        {
            var result = await Handler.Handle(new BuildLoginViewModelRequest() {returnUrl = "https://returnurl"}, CancellationToken.None);
            result.Should().BeOfType<LoginViewModel>();
            result.ServiceName.Should().Be("Acme Service");
            result.ServiceSupportUrl.Should().Be("https://acme.gov.uk/Support");
            result.ReturnUrl.Should().Be("https://returnurl");
            result.ClientId.Should().Be(ClientId);
            result.CreateAccount.LocalSignUp.Should().BeFalse();
        }
    }
}