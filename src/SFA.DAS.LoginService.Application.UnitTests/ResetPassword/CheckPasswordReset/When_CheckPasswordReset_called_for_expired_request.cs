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
    public class When_CheckPasswordReset_called_for_expired_request : CheckPasswordResetTestBase
    {
        [Test]
        public async Task Then_result_IsValid_should_be_false()
        {
            SystemTime.UtcNow = () => new DateTime(2018,1,1,1,1,0);
            
            var requestId = Guid.NewGuid();

            LoginContext.ResetPasswordRequests.Add(new ResetPasswordRequest() {Id = requestId, ValidUntil = SystemTime.UtcNow().AddHours(-1), IsComplete = false, Email = "email@emailaddress.com"});
            await LoginContext.SaveChangesAsync();
            
            var result = await Handler.Handle(new CheckPasswordResetRequest(){RequestId = requestId}, CancellationToken.None);
            result.IsValid.Should().BeFalse();
        }
    }
}