using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.CallbackServiceTests
{
    [TestFixture]
    public class When_calling_callback
    {
        private LoginContext _loginContext;
        private MockHttpMessageHandler _mockHttp;

        [SetUp]
        public void SetUp()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LoginContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _loginContext = new LoginContext(dbContextOptions);
            _mockHttp = new MockHttpMessageHandler();
        }
        [Test]
        public void Then_correct_json_is_posted_to_callback_uri()
        {
            _mockHttp.Expect("https://localhost/callback").WithContent(JsonConvert.SerializeObject(new {sub = "LOGINUSERID", sourceId = "S0U4C31D", inviterId = "INVITERID" }));
            
            var callbackService = new CallbackService(_mockHttp.ToHttpClient(), _loginContext);

            var invitation = new Invitation
            {
                SourceId = "S0U4C31D",
                CallbackUri = new Uri("https://localhost/callback"),
                InviterId = "INVITERID"
            };
            
            callbackService.Callback(invitation, "LOGINUSERID");
            
            _mockHttp.VerifyNoOutstandingExpectation();
        }
        
        [Test]
        public async Task Then_ClientCallBack_user_log_entry_is_created()
        {
            _mockHttp.When("https://localhost/callback").Respond(HttpStatusCode.OK);
            
            var callbackService = new CallbackService(_mockHttp.ToHttpClient(), _loginContext);

            var invitation = new Invitation
            {
                Email = "email@provider.com",
                SourceId = "S0U4C31D",
                CallbackUri = new Uri("https://localhost/callback"),
                InviterId = "INVITERID"
            };
            
            SystemTime.UtcNow = () => new DateTime(2019, 1, 1, 1, 1, 1);
            var logId = Guid.NewGuid();
            GuidGenerator.NewGuid = () => logId;
            
            await callbackService.Callback(invitation, "LOGINUSERID");
            
            var logEntry = _loginContext.UserLogs.Single();

            var expectedLogEntry = new UserLog
            {
                Id = logId,
                DateTime = SystemTime.UtcNow(),
                Email = "email@provider.com",
                Action = "Client callback",
                Result = "Callback OK",
                ExtraData = JsonConvert.SerializeObject(new {CallbackUri="https://localhost/callback", SourceId = "S0U4C31D", UserId = "LOGINUSERID"})
            };

            logEntry.Should().BeEquivalentTo(expectedLogEntry);
        }
        
        [Test]
        public async Task And_Callback_does_not_return_200_Then_ClientCallBack_user_log_entry_is_created()
        {
            _mockHttp.When("https://localhost/callback").Respond(HttpStatusCode.BadRequest,"application/json","Error content");
            
            var callbackService = new CallbackService(_mockHttp.ToHttpClient(), _loginContext);

            var invitation = new Invitation
            {
                Email = "email@provider.com",
                SourceId = "S0U4C31D",
                CallbackUri = new Uri("https://localhost/callback")
            };
            
            SystemTime.UtcNow = () => new DateTime(2019, 1, 1, 1, 1, 1);
            var logId = Guid.NewGuid();
            GuidGenerator.NewGuid = () => logId;
            
            await callbackService.Callback(invitation, "LOGINUSERID");
            
            var logEntry = _loginContext.UserLogs.Single();

            var expectedLogEntry = new UserLog
            {
                Id = logId,
                DateTime = SystemTime.UtcNow(),
                Email = "email@provider.com",
                Action = "Client callback",
                Result = "Callback error",
                ExtraData = JsonConvert.SerializeObject(new {CallbackUri="https://localhost/callback", SourceId = "S0U4C31D", 
                    UserId = "LOGINUSERID",     
                    ResponseStatusCode = 400, 
                    Content = "Error content"})
            };

            logEntry.Should().BeEquivalentTo(expectedLogEntry);
        }

        [Test]
        public void Then_invitation_CalledBack_fields_are_updated()
        {
            _mockHttp.When("https://localhost/callback").Respond(HttpStatusCode.OK);
            
            var invitationId = Guid.NewGuid();
            var invitation = new Invitation
            {
                Id = invitationId,
                SourceId = "S0U4C31D",
                CallbackUri = new Uri("https://localhost/callback")
            };

            _loginContext.Invitations.Add(invitation);
            _loginContext.SaveChanges();
            
            var callbackService = new CallbackService(_mockHttp.ToHttpClient(), _loginContext);
            

            SystemTime.UtcNow = () => new DateTime(2018, 2, 27, 9, 0, 0);
            
            callbackService.Callback(invitation, "LOGINUSERID").Wait();

            var updatedInvitation = _loginContext.Invitations.Single(i => i.Id == invitationId);
            updatedInvitation.IsCalledBack.Should().BeTrue();
            updatedInvitation.CallbackDate.Should().Be(new DateTime(2018, 2, 27, 9, 0, 0));
        }

        [Test]
        public void And_callback_target_does_not_return_200_Then_callback_fields_are_not_updated()
        {
            _mockHttp.When("https://localhost/callback").Respond(HttpStatusCode.BadRequest);
            
            var invitationId = Guid.NewGuid();
            var invitation = new Invitation
            {
                Id = invitationId,
                SourceId = "S0U4C31D",
                CallbackUri = new Uri("https://localhost/callback")
            };

            _loginContext.Invitations.Add(invitation);
            _loginContext.SaveChanges();
            
            var callbackService = new CallbackService(_mockHttp.ToHttpClient(), _loginContext);
            

            SystemTime.UtcNow = () => new DateTime(2018, 2, 27, 9, 0, 0);
            
            callbackService.Callback(invitation, "LOGINUSERID");

            var updatedInvitation = _loginContext.Invitations.Single(i => i.Id == invitationId);
            updatedInvitation.IsCalledBack.Should().BeFalse();
            updatedInvitation.CallbackDate.Should().BeNull();
        }

        [Test]
        public void And_callback_target_url_cannot_be_resolved_Then_callback_fields_are_not_updated()
        {
            _mockHttp.When("https://localhost/callback").Throw(new HttpRequestException());
            
            var invitationId = Guid.NewGuid();
            var invitation = new Invitation
            {
                Id = invitationId,
                SourceId = "S0U4C31D",
                CallbackUri = new Uri("https://localhost/callback")
            };

            _loginContext.Invitations.Add(invitation);
            _loginContext.SaveChanges();
            
            var callbackService = new CallbackService(_mockHttp.ToHttpClient(), _loginContext);
            

            SystemTime.UtcNow = () => new DateTime(2018, 2, 27, 9, 0, 0);

            callbackService.Invoking(cs => cs.Callback(invitation, "LOGINUSERID")).Should().NotThrow<HttpRequestException>();
            
            var updatedInvitation = _loginContext.Invitations.Single(i => i.Id == invitationId);
            updatedInvitation.IsCalledBack.Should().BeFalse();
            updatedInvitation.CallbackDate.Should().BeNull();
        }
        
        [Test]
        public async Task And_callback_target_url_cannot_be_resolved_Then_callback_error_log_entry_inserted()
        {
            _mockHttp.When("https://localhost/callback").Throw(new HttpRequestException());
            
            var invitationId = Guid.NewGuid();
            var invitation = new Invitation
            {
                Email = "email@provider.com",
                Id = invitationId,
                SourceId = "S0U4C31D",
                CallbackUri = new Uri("https://localhost/callback")
            };

            _loginContext.Invitations.Add(invitation);
            _loginContext.SaveChanges();
            
            var callbackService = new CallbackService(_mockHttp.ToHttpClient(), _loginContext);
            
            SystemTime.UtcNow = () => new DateTime(2019, 1, 1, 1, 1, 1);
            var logId = Guid.NewGuid();
            GuidGenerator.NewGuid = () => logId;
            
            await callbackService.Callback(invitation, "LOGINUSERID");
            
            var logEntry = _loginContext.UserLogs.Single();

            var expectedLogEntry = new UserLog
            {
                Id = logId,
                DateTime = SystemTime.UtcNow(),
                Email = "email@provider.com",
                Action = "Client callback",
                Result = "Callback error",
                ExtraData = JsonConvert.SerializeObject(new {CallbackUri="https://localhost/callback", SourceId = "S0U4C31D", 
                    UserId = "LOGINUSERID", 
                    Content = "Exception of type 'System.Net.Http.HttpRequestException' was thrown."})
            };

            logEntry.Should().BeEquivalentTo(expectedLogEntry);
        }
    }
}