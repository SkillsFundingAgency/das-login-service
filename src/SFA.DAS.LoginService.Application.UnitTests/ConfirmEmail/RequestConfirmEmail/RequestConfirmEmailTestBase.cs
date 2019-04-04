using System;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ConfirmEmail;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Application.Services.Configuration;
using SFA.DAS.LoginService.Data;

namespace SFA.DAS.LoginService.Application.UnitTests.ConfirmEmail.RequestConfirmEmail
{
    [TestFixture]
    public class RequestConfirmEmailTestBase
    {
        protected RequestConfirmEmailHandler Handler;
        protected IEmailService EmailService;
        protected ILoginConfig LoginConfig;
        protected LoginContext LoginContext;
        protected IUserService UserService;
        protected IClientService ClientService;
        
        [SetUp]
        public void SetUp()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LoginContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).EnableSensitiveDataLogging()
                .Options;

            EmailService = Substitute.For<IEmailService>();
            LoginConfig = Substitute.For<ILoginConfig>();
            LoginContext = new LoginContext(dbContextOptions);
            UserService = Substitute.For<IUserService>();
            ClientService = Substitute.For<IClientService>();

            Handler = new RequestConfirmEmailHandler(EmailService, LoginConfig, LoginContext, UserService, ClientService);
            
            SystemTime.UtcNow = () => new DateTime(2018,1,1,1,1,0);
        }
    }
}