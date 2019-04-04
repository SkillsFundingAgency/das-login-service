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
    public class When_CheckExistsConfirmEmail_called_for_valid_request : CheckExistsConfirmEmailTestBase
    {
        [SetUp]
        public async Task Arrange()
        {             
            LoginContext.ConfirmEmailRequests.Add(new ConfirmEmailRequest() { Id = Guid.NewGuid(), ValidUntil = SystemTime.UtcNow().AddHours(1), IsComplete = false, Email = "email+one@emailaddress.com" });
            LoginContext.ConfirmEmailRequests.Add(new ConfirmEmailRequest() { Id = Guid.NewGuid(), ValidUntil = SystemTime.UtcNow().AddHours(1), IsComplete = true, Email = "email+two@emailaddress.com" });
            await LoginContext.SaveChangesAsync();
        }

        [Test]
        public async Task Then_result_IsValid_should_be_true_And_result_HasRequest_should_be_true()
        {
            var result = await Handler.Handle(new CheckExistsConfirmEmailRequest() { Email = "email+one@emailaddress.com" }, CancellationToken.None);

            result.IsValid.Should().BeTrue();
            result.HasRequest.Should().BeTrue();
        }
    }
}