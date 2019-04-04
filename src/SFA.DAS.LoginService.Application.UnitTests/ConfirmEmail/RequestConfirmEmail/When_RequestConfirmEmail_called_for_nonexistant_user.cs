using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ConfirmEmail;
using SFA.DAS.LoginService.Application.Services.EmailServiceViewModels;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Data.JsonObjects;

namespace SFA.DAS.LoginService.Application.UnitTests.ConfirmEmail.RequestConfirmEmail
{
    public class When_RequestConfirmEmail_called_for_nonexistant_user : RequestConfirmEmailTestBase
    {
        [SetUp]
        public async Task Arrange()
        {
            UserService.FindByEmail(Arg.Any<string>()).Returns(Task.FromResult<LoginUser>(null));
            LoginContext.Clients.Add(new Client()
            {
                Id = Guid.NewGuid(), 
                ServiceDetails = new ServiceDetails()
                {
                    PostPasswordResetReturnUrl = "https://returnurl", 
                    EmailTemplates = new List<EmailTemplate>()
                    {
                        new EmailTemplate(){Name = "PasswordResetNoAccount", TemplateId = Guid.NewGuid()},
                        new EmailTemplate(){Name = "PasswordReset", TemplateId = Guid.NewGuid()},
                        new EmailTemplate(){Name = "SignUpInvitation", TemplateId = Guid.NewGuid()},
                        new EmailTemplate(){Name = "ConfirmEmail", TemplateId = Guid.NewGuid()},
                    }
                }
            });
            await LoginContext.SaveChangesAsync();
        }
        
        [Test]
        public async Task Then_a_confirm_email_email_is_not_sent()
        {   
            await Handler.Handle(new RequestConfirmEmailRequest() {Email = "nonexistantuser@emailaddress.com", ReturnUrl = "https://returnurl" }, CancellationToken.None);
            
            await EmailService.DidNotReceive().SendEmailConfirmation(Arg.Any<EmailConfirmationEmailViewModel>());
        }
    }
}