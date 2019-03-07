using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.ResetPassword
{
    [TestFixture]
    public class When_ResetPasswordCode_called_for_valid_account : ResetPasswordCodeTestBase
    {
        private async Task Act()
        {
            await Handler.Handle(new ResetPasswordCodeRequest() {Email = "email@emailaddress.com", ClientId = ClientId},
                CancellationToken.None);
        }
        
        [Test]
        public async Task Then_a_reset_password_email_is_sent_to_the_correct_user()
        {
            await Act();

            await EmailService.Received().SendResetPassword("email@emailaddress.com", Arg.Any<string>(), Arg.Any<string>() );
        }

        [Test]
        public async Task Then_a_reset_password_email_is_sent_with_the_correct_code()
        {
            CodeGenerationService.GenerateCode().Returns("ABC123");

            await Act();
            
            await EmailService.Received().SendResetPassword(Arg.Any<string>(), "ABC123", Arg.Any<string>() );
        }
        
        [Test]
        public async Task Then_a_reset_password_email_is_sent_with_the_correct_link()
        {
            await Act();

            var resetPasswordRequest = LoginContext.ResetPasswordRequests.Single();
            
            await EmailService.Received().SendResetPassword(Arg.Any<string>(), Arg.Any<string>(), $"https://baseurl/{ClientId}/{resetPasswordRequest.Id}" );
        }

        [Test]
        public async Task Then_the_correct_ResetPasswordRequest_is_saved_in_the_database()
        {
            SystemTime.UtcNow = () => new DateTime(2019, 1, 1, 10, 0, 0);
            
            HashingService.GetHash(Arg.Any<string>()).Returns("HashedCode");
            
            await Act();

            var resetPasswordRequest = LoginContext.ResetPasswordRequests.Single();

            resetPasswordRequest.ClientId.Should().Be(ClientId);
            resetPasswordRequest.Code.Should().Be("HashedCode");
            resetPasswordRequest.ValidUntil.Should().Be(SystemTime.UtcNow().AddHours(1));
            resetPasswordRequest.IsComplete.Should().BeFalse();
        }
    }
}