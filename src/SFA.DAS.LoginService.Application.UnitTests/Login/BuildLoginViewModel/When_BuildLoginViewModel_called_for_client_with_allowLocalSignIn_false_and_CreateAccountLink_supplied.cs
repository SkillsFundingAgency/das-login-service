using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.BuildLoginViewModel;
using SFA.DAS.LoginService.Data.JsonObjects;

namespace SFA.DAS.LoginService.Application.UnitTests.Login.BuildLoginViewModel
{
    public class When_BuildLoginViewModel_called_for_client_with_allowLocalSignIn_false_and_CreateAccountLink_supplied : BuildLoginViewModelTestBase
    {
        private string _supportUrl = "https://acme.gov.uk/Support";
        private string _createAccountUrl = "https://acme.gov.uk/CreateAccount";
        private string _purpose = "LocalSignUpPurpose";

        [SetUp]
        public async Task Arrange()
        {
            LoginContext.Clients.Add(new Data.Entities.Client()
            {
                IdentityServerClientId = "mvc", 
                ServiceDetails  = new ServiceDetails{ServiceName = "Acme Service", SupportUrl = _supportUrl, CreateAccountUrl = _createAccountUrl, CreateAccountPurpose = _purpose},
                Id = ClientId,
                AllowLocalSignUp = false
            });
            await LoginContext.SaveChangesAsync();
        }

        [Test]
        public async Task Then_LoginViewModel_contains_correct_CreateAccount_details()
        {
            var result = await Handler.Handle(new BuildLoginViewModelRequest{returnUrl = "https://returnurl"}, CancellationToken.None);
            result.CreateAccountDetails.LocalSignUp.Should().BeFalse();
            result.CreateAccountDetails.CreateAccountUrl.Should().Be(_createAccountUrl);
            result.CreateAccountDetails.ShowCreateAccountLink.Should().BeTrue();
            result.CreateAccountDetails.Purpose.Should().Be(_purpose);
        }
    }
}