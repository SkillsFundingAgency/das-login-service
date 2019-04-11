using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.BuildLoginViewModel;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Data.JsonObjects;

namespace SFA.DAS.LoginService.Application.UnitTests.Login.BuildLoginViewModel
{
    public class When_BuildLoginViewModel_called : BuildLoginViewModelTestBase
    {
        [SetUp]
        public async Task Arrange()
        {
            LoginContext.Clients.Add(new Data.Entities.Client()
            {
                IdentityServerClientId = "mvc", 
                ServiceDetails  = new ServiceDetails{ServiceName = "Acme Service", SupportUrl = "https://acme.gov.uk/Support"},
                Id = ClientId
            });
            await LoginContext.SaveChangesAsync();
        }
        
        [Test]
        public async Task Then_LoginViewModel_is_returned()
        {
            var result = await Handler.Handle(new BuildLoginViewModelRequest() {returnUrl = "https://returnurl"}, CancellationToken.None);
            result.Should().BeOfType<LoginViewModel>();
            result.ServiceName.Should().Be("Acme Service");
            result.ServiceSupportUrl.Should().Be("https://acme.gov.uk/Support");
            result.ReturnUrl.Should().Be("https://returnurl");
            result.ClientId.Should().Be(ClientId);
            result.CreateAccountDetails.LocalSignUp.Should().BeFalse();
            result.CreateAccountDetails.ShowCreateAccountLink.Should().BeFalse();
        }
    }
}