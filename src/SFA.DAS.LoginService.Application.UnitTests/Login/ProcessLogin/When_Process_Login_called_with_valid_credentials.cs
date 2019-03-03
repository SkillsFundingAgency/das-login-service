using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ProcessLogin;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.Login.ProcessLogin
{
    public class When_Process_Login_called_with_valid_credentials : ProcessLoginTestBase
    {
        private void Init()
        {   
            UserService.SignInUser(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>()).Returns(SignInResult.Success);
            InteractionService.GetAuthorizationContextAsync(Arg.Any<string>()).Returns(new AuthorizationRequest());
        }
        
        [Test]
        public async Task Then_response_is_returned_with_validcredentials_false()
        {
            Init();
            var result = await Handler.Handle(new ProcessLoginRequest()
                {Username = "user", Password = "password", ReturnUrl = "https://returnurl", RememberLogin = false}, CancellationToken.None);

            result.CredentialsValid.Should().BeTrue();
        }

        [Test]
        public async Task Then_InteractionService_is_asked_for_the_authorization_context()
        {
            Init();
            await Handler.Handle(new ProcessLoginRequest()
                {Username = "user", Password = "password", ReturnUrl = "https://returnurl", RememberLogin = false}, CancellationToken.None);

            await UserService.Received().FindByUsername("user");

            await InteractionService.Received().GetAuthorizationContextAsync("https://returnurl");
        }

        [Test]
        public async Task Then_UserService_is_called_to_get_user_by_username()
        {
            Init();
            await Handler.Handle(new ProcessLoginRequest()
                {Username = "user", Password = "password", ReturnUrl = "https://returnurl", RememberLogin = false}, CancellationToken.None);

            await UserService.Received().FindByUsername("user");
        }
        
        [Test]
        public async Task Then_a_UserLoginSuccessEvent_is_raised_on_the_event_service()
        {
            Init();
            
            var userId = "userid";
            UserService.FindByUsername("user").Returns(new LoginUser() {Id = userId, UserName = "user"});
            
            await Handler.Handle(new ProcessLoginRequest()
                {Username = "user", Password = "password", ReturnUrl = "https://returnurl", RememberLogin = false}, CancellationToken.None);

            await EventService.Received()
                .RaiseAsync(Arg.Is<UserLoginSuccessEvent>(e => e.Username == "user" && e.SubjectId == userId));
        }
    }
}