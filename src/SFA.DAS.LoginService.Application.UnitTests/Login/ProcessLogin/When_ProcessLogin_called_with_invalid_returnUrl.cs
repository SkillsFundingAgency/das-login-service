using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ProcessLogin;

namespace SFA.DAS.LoginService.Application.UnitTests.Login.ProcessLogin
{
    public class When_ProcessLogin_called_with_invalid_returnUrl : ProcessLoginTestBase
    {
        [SetUp]
        public void Arrange()
        {   
            UserService.SignInUser(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>()).Returns(SignInResult.Success);

            // this is always Returns(null) as Default(class) is always null
            InteractionService.GetAuthorizationContextAsync(Arg.Any<string>()).Returns(default(AuthorizationRequest));
        }

        [Test]
        public async Task Then_Response_CredentialsValid_should_be_false()
        {
            // what is the ReturnUrl where does it come from?
            var result = await Handler.Handle(new ProcessLoginRequest()
                {Username = "user", Password = "password", ReturnUrl = "https://returnurl", RememberLogin = false}, CancellationToken.None);

            result.CredentialsValid.Should().BeFalse();
            result.Message.Should().Be("Invalid ReturnUrl");
        }
    }
}