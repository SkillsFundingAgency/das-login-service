using System;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.GetInvitationByIdHandlerTests
{
    public class GetInvitationByIdHandlerTestBase
    {
        private LoginContext _loginContext;
        protected Guid GoodInvitationId;
        protected Invitation GoodInvitation;
        protected Guid ExpiredInvitationId;
        protected Invitation ExpiredInvitation;
        protected GetInvitationByIdHandler Handler;

        [SetUp]
        public void SetUp()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LoginContext>()
                .UseInMemoryDatabase(databaseName: "GetInvitationByIdHandler_tests")
                .Options;

            _loginContext = new LoginContext(dbContextOptions);
            
            InsertGoodInvitation();
            InsertExpiredInvitation();

            Handler = new GetInvitationByIdHandler(_loginContext);
        }

        protected void InsertGoodInvitation()
        {
            GoodInvitationId = Guid.NewGuid();
            GoodInvitation = new Invitation()
            {
                Id = GoodInvitationId,
                Email = "email@address.com",
                FamilyName = "Picard",
                GivenName = "Jean Luc",
                SourceId = Guid.NewGuid().ToString(),
                CallbackUri = new Uri("https://callback"),
                UserRedirectUri = new Uri("https://redirect"),
                ValidUntil = SystemTime.UtcNow().AddHours(1)
            };
            _loginContext.Invitations.Add(GoodInvitation);
            _loginContext.SaveChanges();
        }
        
        protected void InsertExpiredInvitation()
        {
            ExpiredInvitationId = Guid.NewGuid();
            ExpiredInvitation = new Invitation()
            {
                Id = ExpiredInvitationId,
                Email = "email@address.com",
                FamilyName = "Picard",
                GivenName = "Jean Luc",
                SourceId = Guid.NewGuid().ToString(),
                CallbackUri = new Uri("https://callback"),
                UserRedirectUri = new Uri("https://redirect"),
                ValidUntil = SystemTime.UtcNow().AddHours(-1)
            };
            _loginContext.Invitations.Add(ExpiredInvitation);
            _loginContext.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _loginContext.Invitations.RemoveRange(_loginContext.Invitations);
            _loginContext.SaveChanges();
        }
    }
}