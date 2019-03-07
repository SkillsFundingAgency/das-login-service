using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Data;

namespace SFA.DAS.LoginService.Application.UnitTests.ResetPassword
{
    [TestFixture]
    public class ResetPasswordCodeTestBase
    {
        protected IEmailService EmailService;
        protected ICodeGenerationService CodeGenerationService;
        protected ILoginConfig LoginConfig;
        protected ResetPasswordCodeHandler Handler;
        protected Guid ClientId;
        protected LoginContext LoginContext;
        protected IUserService UserService;
        protected IHashingService HashingService;

        [SetUp]
        public async Task SetUp()
        {
            ClientId = Guid.NewGuid();
            EmailService = Substitute.For<IEmailService>();
            CodeGenerationService = Substitute.For<ICodeGenerationService>();
            UserService = Substitute.For<IUserService>();
            LoginConfig = Substitute.For<ILoginConfig>();
            LoginConfig.BaseUrl.Returns("https://baseurl");

            HashingService = Substitute.For<IHashingService>();
            
            var dbContextOptions = new DbContextOptionsBuilder<LoginContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            LoginContext = new LoginContext(dbContextOptions);
            
            Handler = new ResetPasswordCodeHandler(EmailService, CodeGenerationService, LoginConfig, LoginContext, UserService, HashingService);
        }
        
    }
}