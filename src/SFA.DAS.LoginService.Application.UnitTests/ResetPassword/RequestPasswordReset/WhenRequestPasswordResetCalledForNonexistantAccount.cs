using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.UnitTests.ResetPassword.RequestPasswordReset
{
    public class WhenRequestPasswordResetCalledForNonexistantAccount : RequestPasswordResetTestBase
    {
        [SetUp]
        public async Task Arrange()
        {
            UserService.FindByEmail(Arg.Any<string>()).Returns(default(LoginUser));
            LoginContext.Clients.Add(new Client() {Id = ClientId, ServiceDetails = new ServiceDetails() {PostPasswordResetReturnUrl = "https://returnurl"}});
            await LoginContext.SaveChangesAsync();
        }
        
        [Test]
        public async Task Then_a_reset_password_email_is_not_sent()
        {   
            await Handler.Handle(new RequestPasswordResetRequest() {Email = "nonexistantuser@emailaddress.com", ClientId = ClientId}, CancellationToken.None);
            
            await EmailService.DidNotReceive().SendResetPassword("nonexistantuser@emailaddress.com", Arg.Any<string>(), Arg.Any<string>() );
        }
        
        [Test]
        public async Task Then_a_we_dont_have_an_account_for_you_email_is_sent()
        {
            await Handler.Handle(new RequestPasswordResetRequest() {Email = "nonexistantuser@emailaddress.com", ClientId = ClientId}, CancellationToken.None);
            
            await EmailService.Received().SendResetNoAccountPassword("nonexistantuser@emailaddress.com", "https://returnurl");
        }
    }
}