using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ResetPassword;

namespace SFA.DAS.LoginService.Application.UnitTests.ResetPassword.CheckPasswordReset
{
    public class When_CheckPasswordReset_called_for_nonexistant_request : CheckPasswordResetTestBase
    {
        [Test]
        public async Task Then_result_IsValid_should_be_false()
        {
            var result = await Handler.Handle(new CheckPasswordResetRequest(){RequestId = Guid.NewGuid()}, CancellationToken.None);
            result.IsValid.Should().BeFalse();
        }
    }
}