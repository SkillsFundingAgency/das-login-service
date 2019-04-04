using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ConfirmEmail;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.ConfirmEmail.CheckExistsConfirmEmail
{
    public class When_CheckExistsConfirmEmail_called_for_nonexistant_request : CheckExistsConfirmEmailTestBase
    {
        [SetUp]
        public async Task Arrange()
        {             
            LoginContext.ConfirmEmailRequests.Add(new ConfirmEmailRequest() { Id = Guid.NewGuid(), ValidUntil = SystemTime.UtcNow().AddHours(1), IsComplete = false, Email = "email+one@emailaddress.com" });
            LoginContext.ConfirmEmailRequests.Add(new ConfirmEmailRequest() { Id = Guid.NewGuid(), ValidUntil = SystemTime.UtcNow().AddHours(1), IsComplete = false, Email = "email+two@emailaddress.com" });
            await LoginContext.SaveChangesAsync();
        }

        [Test]
        public async Task Then_result_IsValid_should_be_false_And_result_HasRequest_should_be_false()
        {
            var result = await Handler.Handle(new CheckExistsConfirmEmailRequest() { Email = "email+three@emailaddress.com" }, CancellationToken.None);

            result.IsValid.Should().BeFalse();
            result.HasRequest.Should().BeFalse();
        }
    }
}