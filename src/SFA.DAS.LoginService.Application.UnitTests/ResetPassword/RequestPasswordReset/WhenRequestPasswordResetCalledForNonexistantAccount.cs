using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Data.JsonObjects;

namespace SFA.DAS.LoginService.Application.UnitTests.ResetPassword.RequestPasswordReset
{
    public class WhenRequestPasswordResetCalledForNonexistantAccount : RequestPasswordResetTestBase
    {
        [SetUp]
        public async Task Arrange()
        {
            UserService.FindByEmail(Arg.Any<string>()).Returns(default(LoginUser));
            LoginContext.Clients.Add(new Client()
            {
                Id = ClientId, 
                ServiceDetails = new ServiceDetails()
                {
                    PostPasswordResetReturnUrl = "https://returnurl", 
                    EmailTemplates = new List<EmailTemplate>()
                    {
                        new EmailTemplate(){Name = "PasswordResetNoAccount", TemplateId = Guid.NewGuid()},
                        new EmailTemplate(){Name = "PasswordReset", TemplateId = Guid.NewGuid()},
                        new EmailTemplate(){Name = "SignUpInvitation", TemplateId = Guid.NewGuid()},
                    }
                }
            });
            await LoginContext.SaveChangesAsync();
        }
        
        [Test]
        public async Task Then_a_reset_password_email_is_not_sent()
        {   
            await Handler.Handle(new RequestPasswordResetRequest() {Email = "nonexistantuser@emailaddress.com", ClientId = ClientId}, CancellationToken.None);
            
            await EmailService.DidNotReceive().SendResetPassword(Arg.Any<PasswordResetEmailViewModel>());
        }
        
        [Test]
        public async Task Then_a_we_dont_have_an_account_for_you_email_is_sent()
        {
            await Handler.Handle(new RequestPasswordResetRequest() {Email = "nonexistantuser@emailaddress.com", ClientId = ClientId}, CancellationToken.None);
            
            await EmailService.Received().SendResetNoAccountPassword(Arg.Is<PasswordResetNoAccountEmailViewModel>(vm => vm.EmailAddress == "nonexistantuser@emailaddress.com"));
        }
    }
}