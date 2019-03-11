using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Data.JsonObjects;

namespace SFA.DAS.LoginService.Application.UnitTests.ResetPassword.ResetPassword
{
    [TestFixture]
    public class When_ResetPassword_called_with_invalid_password : ResetPasswordTestBase
    {
        [SetUp]
        public async Task Arrange()
        {
            LoginContext.ResetPasswordRequests.Add(new ResetPasswordRequest {Id = RequestId, Email = "email@address.com", ClientId = ClientId, IdentityToken = "T0k3n", IsComplete = false, ValidUntil = SystemTime.UtcNow().AddHours(1)});
            LoginContext.Clients.Add(new Client() {Id = ClientId, ServiceDetails = new ServiceDetails() {PostPasswordResetReturnUrl = "http://returnurl"}});
            await LoginContext.SaveChangesAsync();
            UserService.ResetPassword("email@address.com", "Password", "T0k3n").Returns(new UserResponse(){Result = IdentityResult.Failed()});
        }

        [Test]
        public async Task Then_return_issuccessful_is_false()
        {
            var result = await Handler.Handle(new ResetUserPasswordRequest {ClientId = ClientId, RequestId = RequestId, Password = "Password"}, CancellationToken.None);
            result.IsSuccessful.Should().BeFalse();
        }
    }
}