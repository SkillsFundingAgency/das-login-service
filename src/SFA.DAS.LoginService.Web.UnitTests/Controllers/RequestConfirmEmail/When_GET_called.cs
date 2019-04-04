using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.LoginService.Web.Controllers.ConfirmEmail;
using SFA.DAS.LoginService.Web.Controllers.ConfirmEmail.ViewModels;
using NSubstitute;
using SFA.DAS.LoginService.Application.ConfirmEmail;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.ConfirmEmail
{
    public class When_GET_called : RequentConfirmEmailControllerTestBase
    {
        private string email = "email@email.com";
        private string returnUrl = "http://localhost/returnurl";
        
        [Test]
        public async Task Then_ViewResult_is_returned()
        {
            var result = await Controller.Get(returnUrl, email);
            result.Should().BeOfType<ViewResult>();
            result.As<ViewResult>().ViewName.Should().Be("ConfirmEmailSent");
            result.As<ViewResult>().Model.Should().BeOfType<ConfirmEmailSentViewModel>();
            result.As<ViewResult>().Model.As<ConfirmEmailSentViewModel>().Email.Should().Be(email);
        }

        [Test]
        public async Task Then_RequestConfirmEmail_is_requested()
        {
            await Controller.Get(returnUrl, email);

            await Mediator.Received(1).Send(Arg.Is<RequestConfirmEmailRequest>(p => p.Email == email && p.ReturnUrl == returnUrl));
        }
    }
}