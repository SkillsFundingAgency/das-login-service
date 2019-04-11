using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.CreateAccount;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.CreateAccount
{
    public class When_CreateAccount_called_for_username_which_exists : CreateAccountTestBase
    {
        private string _email = "email+one@emailaddress.com";

        [SetUp]
        public void Arrange()
        {             
            UserService.FindByUsername(Arg.Is(_email)).Returns(new LoginUser() { Email = _email });
        }

        [Test]
        public async Task Then_result_CreateAccountResult_should_be_UsernameAlreadyTaken()
        {
            var result = await Handler.Handle(new CreateAccountRequest() { Username = _email }, CancellationToken.None);

            result.CreateAccountResult.Should().Be(CreateAccountResult.UsernameAlreadyTaken);
        }
    }
}