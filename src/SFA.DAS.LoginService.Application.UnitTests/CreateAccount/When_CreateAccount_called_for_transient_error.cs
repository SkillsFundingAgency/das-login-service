using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.CreateAccount;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.CreateAccount
{
    public class When_CreateAccount_called_for_transient_error : CreateAccountTestBase
    {
        private string _email = "email+one@emailaddress.com";
        private string _password = "AValidPassw0rd1";

        [SetUp]
        public void Arrange()
        {
            UserService.FindByEmail(Arg.Is(_email)).Returns(Task.FromResult<LoginUser>(null));
            UserService.ValidatePassword(Arg.Is(_password)).Returns(Task.FromResult(IdentityResult.Success));
            UserService.CreateUser(Arg.Any<LoginUser>(), Arg.Any<string>()).Returns(Task.FromResult(new UserResponse() { User = null, Result = IdentityResult.Failed() }));
        }

        [Test]
        public async Task Then_result_CreateAccountResult_should_be_UnableToCreateAccount()
        {
            var result = await Handler.Handle(new CreateAccountRequest() { Username = _email, Password = _password }, CancellationToken.None);

            result.CreateAccountResult.Should().Be(CreateAccountResult.UnableToCreateAccount);
            result.Message.Should().Be("The account could not be created at this time");
        }
    }
}