using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Configuration;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.ResetPassword.RequestPasswordReset
{
    [TestFixture]
    public class RequestPasswordResetTestBase
    {
        protected IEmailService EmailService;
        protected ILoginConfig LoginConfig;
        protected RequestPasswordResetHandler Handler;
        protected Guid ClientId;
        protected LoginContext LoginContext;
        protected IUserService UserService;

        [SetUp]
        public void SetUp()
        {
            ClientId = Guid.NewGuid();
            EmailService = Substitute.For<IEmailService>();
            
            
            UserService = Substitute.For<IUserService>();
            UserService.GeneratePasswordResetToken(Arg.Any<LoginUser>()).Returns("Token");
            
            LoginConfig = Substitute.For<ILoginConfig>();
            LoginConfig.BaseUrl.Returns("https://baseurl");
            LoginConfig.PasswordResetExpiryInHours = 1;
            
            
            var dbContextOptions = new DbContextOptionsBuilder<LoginContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            LoginContext = new LoginContext(dbContextOptions);
            
            Handler = new RequestPasswordResetHandler(EmailService, LoginConfig, LoginContext, UserService);
        }
        
    }
}