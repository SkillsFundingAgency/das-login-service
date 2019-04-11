using System;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.CreateAccount;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Data;

namespace SFA.DAS.LoginService.Application.UnitTests.CreateAccount
{
    [TestFixture]
    public class CreateAccountTestBase
    {
        protected CreateAccountHandler Handler;
        protected LoginContext LoginContext;
        protected IUserService UserService;

        [SetUp]
        public void SetUp()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LoginContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).EnableSensitiveDataLogging()
                .Options;

            LoginContext = new LoginContext(dbContextOptions);
            UserService = Substitute.For<IUserService>();

            Handler = new CreateAccountHandler(LoginContext, UserService);
        }
    }
}