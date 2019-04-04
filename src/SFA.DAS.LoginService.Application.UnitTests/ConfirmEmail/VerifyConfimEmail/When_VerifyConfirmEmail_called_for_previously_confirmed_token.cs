using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ConfirmEmail;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Application.UnitTests.ConfirmEmail.VerifyConfirmEmail;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.ConfirmEmail
{
    public class When_VerifyConfirmEmail_called_for_previously_confirmed_token : VerifyConfirmEmailTestBase
    {
        private string _identityToken = "12345+67890/ABCDE+FGHIJK/LMNOP+QRSTU/VWXYZ=";

        [SetUp]
        public async Task Arrange()
        {
            var currentRequestIds = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
            LoginContext.ConfirmEmailRequests.Add(new ConfirmEmailRequest() { Id = currentRequestIds[0], Email = "email+one@emailaddress.com", ValidUntil = SystemTime.UtcNow().AddMinutes(10), IsComplete = true, IdentityToken = "ABC+" + _identityToken });
            LoginContext.ConfirmEmailRequests.Add(new ConfirmEmailRequest() { Id = currentRequestIds[1], Email = "email+two@emailaddress.com", ValidUntil = SystemTime.UtcNow().AddMinutes(20), IsComplete = true, IdentityToken = _identityToken });
            LoginContext.ConfirmEmailRequests.Add(new ConfirmEmailRequest() { Id = currentRequestIds[2], Email = "email_three@emailaddress.com", ValidUntil = SystemTime.UtcNow().AddMinutes(30), IsComplete = false, IdentityToken = "123+" + _identityToken });
            await LoginContext.SaveChangesAsync();
        }

        [Test]
        public async Task Then_the_correct_response_is_returned()
        {
            var response = await Handler.Handle(new VerifyConfirmEmailRequest() { IdentityToken = _identityToken },
                CancellationToken.None);

            response.VerifyConfirmedEmailResult.Should().Be(VerifyConfirmedEmailResult.TokenPreviouslyVerified);
        }
    }
}