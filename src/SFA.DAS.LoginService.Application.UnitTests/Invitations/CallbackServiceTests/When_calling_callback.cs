using System;
using Newtonsoft.Json;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.CallbackServiceTests
{
    [TestFixture]
    public class When_calling_callback
    {
        [Test]
        public void Then_correct_json_is_posted_to_callback_uri()
        {
            var mockHttp = new MockHttpMessageHandler();
            
            mockHttp.Expect("https://localhost/callback").WithContent(JsonConvert.SerializeObject(new {sub = "LOGINUSERID", sourceId = "S0U4C31D"}));
            
            var callbackService = new CallbackService(mockHttp.ToHttpClient());

            var invitation = new Invitation
            {
                SourceId = "S0U4C31D",
                CallbackUri = new Uri("https://localhost/callback")
            };
            
            callbackService.Callback(invitation, "LOGINUSERID");
            
            mockHttp.VerifyNoOutstandingExpectation();
        }
    }
}