using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Invitations.CreateInvitation;
using SFA.DAS.LoginService.Configuration;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Data.JsonObjects;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.CreateInvitationHandlerTests
{
    [TestFixture]
    public class CreateInvitationHandlerTestBase
    {
        protected CreateInvitationHandler CreateInvitationHandler;
        protected LoginContext LoginContext; 
        protected IEmailService EmailService;
        protected ILoginConfig LoginConfig;
        protected IUserService UserService;
        protected static Guid ClientId;
        protected Guid InvitationTemplateId;

        [SetUp]
        public void SetUp()
        {
            BuildLoginContext();

            EmailService = Substitute.For<IEmailService>();
            LoginConfig = Substitute.For<ILoginConfig>();
            LoginConfig.BaseUrl.Returns("https://baseurl");
            UserService = Substitute.For<IUserService>();
            
            CreateInvitationHandler = BuildCreateInvitationHandler();
        }

        private void BuildLoginContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LoginContext>()
                .UseInMemoryDatabase(databaseName: "invitations_tests")
                .Options;

            LoginContext = new LoginContext(dbContextOptions);
            ClientId = Guid.NewGuid();
            InvitationTemplateId = Guid.Parse("a2fc2212-253e-47c1-b847-27c10f83f7f5");
            LoginContext.Clients.Add(new Client()
            {
                Id = ClientId, 
                AllowInvitationSignUp = true, 
                ServiceDetails = new ServiceDetails()
                {
                    ServiceName = "Acme Service", 
                    ServiceTeam = "Acme Service Team",
                    PostPasswordResetReturnUrl = "https://serviceurl",
                    EmailTemplates = new List<EmailTemplate>(){new EmailTemplate(){Name="SignUpInvitation", TemplateId = InvitationTemplateId},new EmailTemplate(){Name="LoginSignupError", TemplateId = InvitationTemplateId}}
                }
            });
            LoginContext.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            LoginContext.Invitations.RemoveRange(LoginContext.Invitations);
            LoginContext.UserLogs.RemoveRange(LoginContext.UserLogs);
            LoginContext.SaveChanges();
        }

        protected static CreateInvitationRequest BuildCreateInvitationRequest()
        {
            var createInvitationRequest = new CreateInvitationRequest()
            {
                Email = "invited@email.com",
                GivenName = "InvitedGivenName",
                FamilyName = "InvitedFamilyName",
                SourceId = "InvitedSourceId",
                UserRedirect = new Uri("https://localhost/userredirect"),
                Callback = new Uri("https://localhost/callback"),
                ClientId = ClientId,
                Inviter = "Inviter",
                InviterEmail = "inviter@email.com"
            };
            return createInvitationRequest;
        }
        
        protected static CreateInvitationRequest BuildEmptyCreateInvitationRequest()
        {
            var createInvitationRequest = new CreateInvitationRequest();
            return createInvitationRequest;
        }

        private CreateInvitationHandler BuildCreateInvitationHandler()
        {
            return new CreateInvitationHandler(LoginContext, EmailService, LoginConfig, UserService, Substitute.For<ILogger<CreateInvitationHandler>>());
        }
    }
}