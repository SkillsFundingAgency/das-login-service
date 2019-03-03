using IdentityServer4.Services;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.ProcessLogin;

namespace SFA.DAS.LoginService.Application.UnitTests.Login.ProcessLogin
{
    [TestFixture]
    public class ProcessLoginTestBase
    {
        protected IUserService UserService;
        protected ProcessLoginHandler Handler;
        protected IEventService EventService;
        protected IIdentityServerInteractionService InteractionService;

        [SetUp]
        public void SetUp()
        {
            EventService = Substitute.For<IEventService>();
            UserService = Substitute.For<IUserService>();
            InteractionService = Substitute.For<IIdentityServerInteractionService>();
            Handler = new ProcessLoginHandler(UserService, EventService, InteractionService);
        }
    }
}