using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.ProcessLogin;

namespace SFA.DAS.LoginService.Application.UnitTests.Login.ProcessLogin
{
    public class When_process_Login_called_for_locked_out_user : ProcessLoginTestBase
    {
        [SetUp]
        public void Arrange()
        {
            UserService.SignInUser(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>()).Returns(SignInResult.LockedOut);
            InteractionService.GetAuthorizationContextAsync(Arg.Any<string>()).Returns(new AuthorizationRequest());
        }
        
        [Test]
        public async Task Then_response_is_returned_with_validcredentials_false()
        {
            var result = await Handler.Handle(new ProcessLoginRequest()
                {Username = "user", Password = "password", ReturnUrl = "https://returnurl", RememberLogin = false}, CancellationToken.None);

            result.CredentialsValid.Should().BeFalse();
        }
        
        [Test]
        public async Task Then_response_is_returned_with_correct_error_message()
        {
            var result = await Handler.Handle(new ProcessLoginRequest()
                {Username = "user", Password = "password", ReturnUrl = "https://returnurl", RememberLogin = false}, CancellationToken.None);

            result.Message.Should().Be("User account is locked out");
        }
    }
}