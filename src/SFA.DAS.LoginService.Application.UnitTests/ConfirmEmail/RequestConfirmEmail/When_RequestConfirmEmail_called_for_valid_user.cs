using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ConfirmEmail;
using SFA.DAS.LoginService.Application.Services;
using SFA.DAS.LoginService.Application.Services.EmailServiceViewModels;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Data.JsonObjects;

namespace SFA.DAS.LoginService.Application.UnitTests.ConfirmEmail.RequestConfirmEmail
{
    [TestFixture]
    public class When_RequestConfirmEmail_called_for_valid_user : RequestConfirmEmailTestBase
    {
        private string _returnUrl = "https://return/url/";
        private string _identityToken = "12345+67890/ABCDE+FGHIJK/LMNOP+QRSTU/VWXYZ=";
        private LoginUser _loginUser;

        [SetUp]
        public void Arrange()
        {
            _loginUser = new LoginUser() { GivenName = "GivenName", Email = "email+givenname@emailaddress.com" };
            UserService.FindByEmail(Arg.Any<string>()).Returns(_loginUser);
            var client = new Client()
            {
                Id = Guid.NewGuid(),
                ServiceDetails = new ServiceDetails()
                {
                    ServiceName = "Test Service",
                    PostPasswordResetReturnUrl = "https://returnurl", 
                    EmailTemplates = new List<EmailTemplate>()
                    {
                        new EmailTemplate(){Name = "PasswordResetNoAccount", TemplateId = Guid.NewGuid()},
                        new EmailTemplate(){Name = "PasswordReset", TemplateId = Guid.NewGuid()},
                        new EmailTemplate(){Name = "SignUpInvitation", TemplateId = Guid.NewGuid()},
                        new EmailTemplate(){Name = "ConfirmEmail", TemplateId = Guid.NewGuid()}
                    }
                }
            };

            LoginConfig.BaseUrl.Returns(new Uri("http://baseurl/").ToString());
            LoginConfig.ConfirmEmailExpiryInHours.Returns(1);
            UserService.GenerateConfirmEmailToken(Arg.Is(_loginUser)).Returns(_identityToken);
            ClientService.GetByReturnUrl(Arg.Is(_returnUrl), Arg.Any<CancellationToken>()).Returns(client);
        }
        
        private async Task Act()
        {
            await Handler.Handle(new RequestConfirmEmailRequest() { Email = _loginUser.Email, ReturnUrl = _returnUrl },
                CancellationToken.None);
        }
        
        [Test]
        public async Task Then_a_confirm_email_is_sent_to_the_correct_user()
        {
            await Act();

            await EmailService.Received(1).SendEmailConfirmation(Arg.Is<EmailConfirmationEmailViewModel>(vm => vm.EmailAddress == _loginUser.Email));
        }
        
        [Test]
        public async Task Then_a_confirm_email_is_sent_with_the_correct_link()
        {
            await Act();

            await EmailService.Received(1).SendEmailConfirmation(Arg.Is<EmailConfirmationEmailViewModel>(
                vm => vm.ConfirmLink == new Uri(new Uri($"{LoginConfig.BaseUrl}"), $"ConfirmEmail/{Uri.EscapeDataString(_returnUrl)}/{Uri.EscapeDataString(_identityToken)}").ToString()));
        }
        
        [Test]
        public async Task Then_a_confirm_email_is_sent_with_the_correct_givenName()
        {
            await Act();

            await EmailService.Received(1).SendEmailConfirmation(Arg.Is<EmailConfirmationEmailViewModel>(vm => vm.Contact == _loginUser.GivenName));
        }

        [Test]
        public async Task Then_the_correct_confirm_email_request_is_saved_in_the_database()
        {
            SystemTime.UtcNow = () => new DateTime(2019, 1, 1, 10, 0, 0);
                      
            await Act();

            var confirmEmailRequest = LoginContext.ConfirmEmailRequests.Single();

            confirmEmailRequest.ValidUntil.Should().Be(SystemTime.UtcNow().AddHours(LoginConfig.ConfirmEmailExpiryInHours));
            confirmEmailRequest.RequestedDate.Should().Be(SystemTime.UtcNow());
            confirmEmailRequest.IsComplete.Should().BeFalse();
            confirmEmailRequest.Email.Should().Be(_loginUser.Email);
            confirmEmailRequest.IdentityToken.Should().Be(_identityToken);
        }

        [Test]
        public async Task Then_previous_valid_confirm_email_requests_are_expired()
        {
            SystemTime.UtcNow = () => new DateTime(2018,1,1,11,11,11);

            var currentRequestIds = new[] {Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()};            
            LoginContext.ConfirmEmailRequests.Add(new ConfirmEmailRequest() { Id = currentRequestIds[0], Email = _loginUser.Email, ValidUntil = SystemTime.UtcNow().AddMinutes(-10), IsComplete = false});
            LoginContext.ConfirmEmailRequests.Add(new ConfirmEmailRequest() { Id = currentRequestIds[1], Email = _loginUser.Email, ValidUntil = SystemTime.UtcNow().AddMinutes(15), IsComplete = false });
            LoginContext.ConfirmEmailRequests.Add(new ConfirmEmailRequest() { Id = currentRequestIds[2], Email = "email+two@emailaddress.com", ValidUntil = SystemTime.UtcNow().AddMinutes(20), IsComplete = false});
            LoginContext.ConfirmEmailRequests.Add(new ConfirmEmailRequest() { Id = currentRequestIds[3], Email = "email+three@emailaddress.com", ValidUntil = SystemTime.UtcNow().AddMinutes(25), IsComplete = true});
            LoginContext.ConfirmEmailRequests.Add(new ConfirmEmailRequest() { Id = currentRequestIds[4], Email = "email_four@emailaddress.com", ValidUntil = SystemTime.UtcNow().AddMinutes(-10), IsComplete = false});
            await LoginContext.SaveChangesAsync();

            await Act();

            var confirmEmailRequestsWithValidExpiry = LoginContext.ConfirmEmailRequests.Where(r => r.Email == _loginUser.Email && r.ValidUntil > SystemTime.UtcNow() && r.IsComplete == false);
            confirmEmailRequestsWithValidExpiry.Count(r => currentRequestIds.Contains(r.Id)).Should().Be(0); // there are no valid original request
            confirmEmailRequestsWithValidExpiry.Count().Should().Be(1); // there is a single valid new request 

            var otherConfirmEmailRequestsWithValidExpiry = LoginContext.ConfirmEmailRequests.Where(r => r.Email != _loginUser.Email && r.ValidUntil > SystemTime.UtcNow() && r.IsComplete == false);
            otherConfirmEmailRequestsWithValidExpiry.Count(r => currentRequestIds.Contains(r.Id)).Should().Be(1); // the remaining one for a different user is still valid
        }
    }
}