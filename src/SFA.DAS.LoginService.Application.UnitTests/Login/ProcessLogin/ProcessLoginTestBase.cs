using System;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.ProcessLogin;
using SFA.DAS.LoginService.Data;

namespace SFA.DAS.LoginService.Application.UnitTests.Login.ProcessLogin
{
    [TestFixture]
    public class ProcessLoginTestBase
    {
        protected IUserService UserService;
        protected ProcessLoginHandler Handler;
        protected IEventService EventService;
        protected IIdentityServerInteractionService InteractionService;
        protected LoginContext LoginContext;
        
        [SetUp]
        public void SetUp()
        {
            EventService = Substitute.For<IEventService>();
            UserService = Substitute.For<IUserService>();
            InteractionService = Substitute.For<IIdentityServerInteractionService>();
            var dbContextOptions = new DbContextOptionsBuilder<LoginContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            LoginContext = new LoginContext(dbContextOptions);
            
            Handler = new ProcessLoginHandler(UserService, EventService, InteractionService, LoginContext);
        }
    }
}