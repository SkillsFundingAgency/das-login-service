using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.ResetPassword.CheckPasswordReset
{
    public class When_CheckPasswordReset_called_for_valid_request : CheckPasswordResetTestBase
    {
        private Guid _requestId;

        [SetUp]
        public async Task Arrange()
        {
            _requestId = Guid.NewGuid();

            LoginContext.ResetPasswordRequests.Add(new ResetPasswordRequest() {Id = _requestId, ValidUntil = SystemTime.UtcNow().AddHours(1), IsComplete = false, Email = "email@emailaddress.com"});
            await LoginContext.SaveChangesAsync();
        }
        
        [Test]
        public async Task Then_result_contains_IsValid_true()
        {
            var result = await Handler.Handle(new CheckPasswordResetRequest(){RequestId = _requestId}, CancellationToken.None);
            result.Should().BeOfType<CheckPasswordResetResponse>();
            result.IsValid.Should().BeTrue();
            result.RequestId.Should().Be(_requestId);
            result.Email.Should().Be("email@emailaddress.com");
        }
    }
}