using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ProcessLogin;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.Login.ProcessLogin
{
    public class When_Process_Login_called : ProcessLoginTestBase
    {
        [Test]
        public async Task Then_UserService_is_asked_to_signIn_user()
        {
            UserService.FindByUsername("user").Returns(new LoginUser());
            UserService.SignInUser(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>()).Returns(SignInResult.Success);
            InteractionService.GetAuthorizationContextAsync(Arg.Any<string>()).Returns(new AuthorizationRequest());
            await Handler.Handle(new ProcessLoginRequest()
                {Username = "user", Password = "password", ReturnUrl = "https://returnurl", RememberLogin = false}, CancellationToken.None);

            await UserService.Received().SignInUser("user", "password", false);
        }
    }
}