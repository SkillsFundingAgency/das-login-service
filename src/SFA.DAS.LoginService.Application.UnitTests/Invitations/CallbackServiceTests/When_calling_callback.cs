using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NSubstitute;
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
                .UseInMemoryDatabase(databaseName: "ConfirmCodeHandler_tests")
                .Options;

            _loginContext = new LoginContext(dbContextOptions);
            _mockHttp = new MockHttpMessageHandler();
        }
        
        [Test]
        public void Then_correct_json_is_posted_to_callback_uri()
        {
            _mockHttp.Expect("https://localhost/callback").WithContent(JsonConvert.SerializeObject(new {sub = "LOGINUSERID", sourceId = "S0U4C31D"}));
            
            var callbackService = new CallbackService(_mockHttp.ToHttpClient(), _loginContext);

            var invitation = new Invitation
            {
                SourceId = "S0U4C31D",
                CallbackUri = new Uri("https://localhost/callback")
            };
            
            callbackService.Callback(invitation, "LOGINUSERID");
            
            _mockHttp.VerifyNoOutstandingExpectation();
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
    }
}