using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.BuildLogoutViewModel;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.UnitTests.Helpers;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;
using LogoutRequest = SFA.DAS.LoginService.Application.BuildLogoutViewModel.LogoutRequest;

namespace SFA.DAS.LoginService.Application.UnitTests.Logout
{
    [TestFixture]
    public class When_LogoutHandler_called
    {
        private IIdentityServerInteractionService _interactionService;
        private LogoutHandler _handler;
        private LoginContext _loginContext;
        private IUserService _userService;
        private IEventService _eventService;
        private IHttpContextAccessor _httpContextAccessor;

        [SetUp]
        public async Task SetUp()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LoginContext>()
                .UseInMemoryDatabase(databaseName: this.GetType().Name)
                .Options;

            _loginContext = new LoginContext(dbContextOptions);
            _loginContext.Clients.Add(new Data.Entities.Client(){IdentityServerClientId = "mvc", ServiceDetails  = new ServiceDetails{ServiceName = "Acme Service", SupportUrl = "https://acme.gov.uk/Support"}});
            await _loginContext.SaveChangesAsync();
            
            _interactionService = Substitute.For<IIdentityServerInteractionService>();
            _interactionService.GetLogoutContextAsync("logoutid").Returns(new IdentityServer4.Models.LogoutRequest("iframeurl", new LogoutMessage()){ClientId = "mvc", PostLogoutRedirectUri = "https://postlogouturi"});

            _userService = Substitute.For<IUserService>();

            var principal = new TestPrincipal(new Claim(JwtClaimTypes.Subject, "user123"));

            _eventService = Substitute.For<IEventService>();

            _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            _httpContextAccessor.HttpContext.User.Returns(principal);
            
            _handler = new LogoutHandler(_interactionService, _loginContext, _userService, _eventService, _httpContextAccessor);
        }
        
        [TearDown]
        public async Task TearDown()
        {
            _loginContext.Clients.RemoveRange(_loginContext.Clients);
            await _loginContext.SaveChangesAsync();
        }
        
        [Test]
        public async Task Then_interaction_service_is_asked_for_LogoutContext()
        {
            await _handler.Handle(new LogoutRequest() {LogoutId = "logoutid"}, CancellationToken.None);
            await _interactionService.Received().GetLogoutContextAsync("logoutid");
        }

        [Test]
        public async Task Then_user_service_signout_is_called()
        {
            await _handler.Handle(new LogoutRequest() {LogoutId = "logoutid"}, CancellationToken.None);
            await _userService.Received().SignOutUser();
        }

        [Test]
        public async Task Then_UserLogoutSuccessEvent_is_raised_on_Eventservice()
        {
            await _handler.Handle(new LogoutRequest() {LogoutId = "logoutid"}, CancellationToken.None);
            await _eventService.Received().RaiseAsync(Arg.Is<UserLogoutSuccessEvent>(e => e.SubjectId == "user123" && e.DisplayName == "User"));
        }
        
        [Test]
        public async Task Then_response_contains_correct_values()
        {
            var result = await _handler.Handle(new LogoutRequest() {LogoutId = "logoutid"}, CancellationToken.None);

            result.PostLogoutRedirectUri.Should().Be("https://postlogouturi");
            result.ClientName.Should().Be("Acme Service");
            result.LogoutId.Should().Be("logoutid");
        }
    }
}