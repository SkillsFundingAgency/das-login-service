using System;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ConfirmEmail;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;

namespace SFA.DAS.LoginService.Application.UnitTests.ConfirmEmail.CheckExistsConfirmEmail
{
    [TestFixture]
    public class CheckExistsConfirmEmailTestBase
    {
        protected CheckExistsConfirmEmailHandler Handler;
        protected LoginContext LoginContext;
        
        [SetUp]
        public void SetUp()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LoginContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).EnableSensitiveDataLogging()
                .Options;

            LoginContext = new LoginContext(dbContextOptions);
            
            Handler = new CheckExistsConfirmEmailHandler(LoginContext);
            
            SystemTime.UtcNow = () => new DateTime(2018,1,1,1,1,0);
        }
    }
}