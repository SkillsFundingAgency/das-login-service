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
    public class When_CreateAccount_called_for_with_no_errors : CreateAccountTestBase
    {
        private string _givenName = "given";
        private string _familyName = "family";
        private string _email = "email+one@emailaddress.com";
        private string _password = "AValidPassw0rd1";

        [SetUp]
        public void Arrange()
        {
            UserService.FindByEmail(Arg.Is(_email)).Returns(Task.FromResult<LoginUser>(null));
            UserService.ValidatePassword(Arg.Is(_password)).Returns(Task.FromResult(IdentityResult.Success));
            UserService.CreateUser(Arg.Any<LoginUser>(), Arg.Any<string>()).Returns(Task.FromResult(new UserResponse()
            {
                User = new LoginUser
                {
                    GivenName = _givenName,
                    FamilyName = _familyName,
                    UserName = _email,
                    Email = _email,
                    EmailConfirmed = false
                },
                Result = IdentityResult.Success
            }));
        }

        [Test]
        public async Task Then_result_CreateAccountResult_should_be_Successful()
        {
            var result = await Handler.Handle(new CreateAccountRequest() { GivenName = _givenName, FamilyName = _familyName, Username = _email, Password = _password }, CancellationToken.None);

            result.CreateAccountResult.Should().Be(CreateAccountResult.Successful);
            result.Message.Should().BeNull();
        }
    }
}