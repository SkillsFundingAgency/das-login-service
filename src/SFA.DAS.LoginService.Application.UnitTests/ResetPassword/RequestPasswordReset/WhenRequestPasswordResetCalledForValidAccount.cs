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

namespace SFA.DAS.LoginService.Application.UnitTests.ResetPassword.RequestPasswordReset
{
    [TestFixture]
    public class WhenRequestPasswordResetCalledForValidAccount : RequestPasswordResetTestBase
    {
        [SetUp]
        public void Arrange()
        {
            UserService.FindByEmail(Arg.Any<string>()).Returns(new LoginUser());
        }
        
        private async Task Act()
        {
            await Handler.Handle(new RequestPasswordResetRequest() {Email = "email@emailaddress.com", ClientId = ClientId},
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
            
            await EmailService.Received().SendResetPassword(Arg.Any<string>(), Arg.Any<string>(), $"https://baseurl/NewPassword/ConfirmCode/{ClientId}/{resetPasswordRequest.Id}" );
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
            resetPasswordRequest.RequestedDate.Should().Be(SystemTime.UtcNow());
            resetPasswordRequest.IsComplete.Should().BeFalse();
            resetPasswordRequest.Email.Should().Be("email@emailaddress.com");
            resetPasswordRequest.IdentityToken.Should().Be("Token");
        }

        [Test]
        public async Task Then_previous_valid_reset_requests_are_expired()
        {
            SystemTime.UtcNow = () => new DateTime(2018,1,1,11,11,11);

            var validRequestIds = new[] {Guid.NewGuid(), Guid.NewGuid()};
            
            LoginContext.ResetPasswordRequests.Add(new ResetPasswordRequest() { Id= validRequestIds[0], Email = "email@emailaddress.com", ValidUntil = SystemTime.UtcNow().AddMinutes(5), IsComplete = false});
            LoginContext.ResetPasswordRequests.Add(new ResetPasswordRequest() { Id= validRequestIds[1], Email = "email@emailaddress.com", ValidUntil = SystemTime.UtcNow().AddMinutes(1), IsComplete = false});
            LoginContext.ResetPasswordRequests.Add(new ResetPasswordRequest() { Id= Guid.NewGuid(), Email = "email@emailaddress.com", ValidUntil = SystemTime.UtcNow().AddMinutes(10), IsComplete = true});
            LoginContext.ResetPasswordRequests.Add(new ResetPasswordRequest() { Id= Guid.NewGuid(), Email = "email@emailaddress.com", ValidUntil = SystemTime.UtcNow().AddMinutes(-10), IsComplete = false});
            await LoginContext.SaveChangesAsync();

            await Act();

            var passwordRequestsWithValidExpiry = LoginContext.ResetPasswordRequests.Where(r => r.Email == "email@emailaddress.com" && r.ValidUntil > SystemTime.UtcNow() && r.IsComplete == false);

            passwordRequestsWithValidExpiry.Count().Should().Be(1);
            passwordRequestsWithValidExpiry.Any(r => validRequestIds.Contains(r.Id)).Should().BeFalse();
        }
    }
}