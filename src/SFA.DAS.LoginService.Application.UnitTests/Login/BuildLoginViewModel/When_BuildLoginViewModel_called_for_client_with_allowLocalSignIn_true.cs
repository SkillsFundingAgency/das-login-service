using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.BuildLoginViewModel;
using SFA.DAS.LoginService.Data.JsonObjects;

namespace SFA.DAS.LoginService.Application.UnitTests.Login.BuildLoginViewModel
{
    public class When_BuildLoginViewModel_called_for_client_with_allowLocalSignIn_true : BuildLoginViewModelTestBase
    {
        [SetUp]
        public async Task Arrange()
        {
            LoginContext.Clients.Add(new Data.Entities.Client()
            {
                IdentityServerClientId = "mvc", 
                ServiceDetails  = new ServiceDetails{ServiceName = "Acme Service", SupportUrl = "https://acme.gov.uk/Support"},
                Id = ClientId,
                AllowLocalSignUp = true
            });
            await LoginContext.SaveChangesAsync();
        }

        [Test]
        public async Task Then_LoginViewModel_contains_correct_CreateAccount_details()
        {
            var result = await Handler.Handle(new BuildLoginViewModelRequest{returnUrl = "https://returnurl"}, CancellationToken.None);
            result.CreateAccount.LocalSignUp.Should().BeTrue();
            result.CreateAccount.ShowCreateAccountLink.Should().BeTrue();
        }
    }
}