using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.BuildLoginViewModel;
using SFA.DAS.LoginService.Data.JsonObjects;

namespace SFA.DAS.LoginService.Application.UnitTests.Login.BuildLoginViewModel
{
    public class When_BuildLoginViewModel_called_for_client_with_allowLocalSignIn_false_and_CreateAccountLinkSupplied : BuildLoginViewModelTestBase
    {
        [SetUp]
        public async Task Arrange()
        {
            LoginContext.Clients.Add(new Data.Entities.Client()
            {
                IdentityServerClientId = "mvc", 
                ServiceDetails  = new ServiceDetails{ServiceName = "Acme Service", SupportUrl = "https://acme.gov.uk/Support", CreateAccountUrl = "https://acme.gov.uk/CreateAccount"},
                Id = ClientId,
                AllowLocalSignUp = false
            });
            await LoginContext.SaveChangesAsync();
        }

        [Test]
        public async Task Then_LoginViewModel_contains_correct_CreateAccount_details()
        {
            var result = await Handler.Handle(new BuildLoginViewModelRequest{returnUrl = "https://returnurl"}, CancellationToken.None);
            result.CreateAccount.LocalSignUp.Should().BeFalse();
            result.CreateAccount.CreateAccountUrl.Should().Be("https://acme.gov.uk/CreateAccount");
            result.CreateAccount.ShowCreateAccountLink.Should().BeTrue();
        }
    }
}