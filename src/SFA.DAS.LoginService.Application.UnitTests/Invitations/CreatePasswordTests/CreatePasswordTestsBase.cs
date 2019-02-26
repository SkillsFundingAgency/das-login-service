using System;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Moq;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.CreatePassword;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.UnitTests.Helpers;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.CreatePasswordTests
{
    [TestFixture]
    public class CreatePasswordTestsBase
    {
        protected IUserService UserService;
        protected LoginContext LoginContext;
        protected Guid InvitationId;
        protected CreatePasswordHandler Handler;
        protected Mock<IBackgroundJobClient> BackgroundJobClient;
        protected Guid NewLoginUserId;

        [SetUp]
        public void SetUp()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LoginContext>()
                .UseInMemoryDatabase(databaseName: "CreatePasswordHandler_tests")
                .Options;

            LoginContext = new LoginContext(dbContextOptions);

            InvitationId = Guid.NewGuid();
            LoginContext.Invitations.Add(new Invitation()
            {
                Id = InvitationId,
                Code = "code".GenerateHash(),
                Email = "email@provider.com",
                SourceId = "ABC123"
            });
            LoginContext.SaveChanges();

            // Using Moq here instead of NSubstitute as it verifies calls to Hangfire's background queue better. 
            BackgroundJobClient = new Mock<IBackgroundJobClient>();

            var callbackService = Substitute.For<ICallbackService>();

            UserService = Substitute.For<IUserService>();
            NewLoginUserId = Guid.NewGuid();
            
            
            Handler = new CreatePasswordHandler(UserService, LoginContext, BackgroundJobClient.Object, callbackService);
        }
    }
}