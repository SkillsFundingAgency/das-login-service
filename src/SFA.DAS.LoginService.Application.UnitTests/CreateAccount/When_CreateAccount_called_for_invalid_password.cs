using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.CreateAccount;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.CreateAccount
{
    public class When_CreateAccount_called_for_invalid_password : CreateAccountTestBase
    {
        private string _email = "email+one@emailaddress.com";
        private string _password = "invalid";

        [SetUp]
        public void Arrange()
        {
            UserService.FindByEmail(Arg.Is(_email)).Returns(Task.FromResult<LoginUser>(null));
            UserService.ValidatePassword(Arg.Is(_password)).Returns(Task.FromResult(IdentityResult.Failed()));
        }

        [Test]
        public async Task Then_result_CreateAccountResult_should_be_PasswordComplexityInvalid()
        {
            var result = await Handler.Handle(new CreateAccountRequest() { Username = _email, Password = _password }, CancellationToken.None);

            result.CreateAccountResult.Should().Be(CreateAccountResult.PasswordComplexityInvalid);
            result.Message.Should().Be("Password does not meet validity rules");
        }
    }
}