using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetClientById;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Data.JsonObjects;
using SFA.DAS.LoginService.Types.GetClientById;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.PasswordReset
{
    public class When_Get_called_on_PasswordResetSuccessful : PasswordResetTestBase
    {
        [Test]
        public async Task Then_correct_ViewResult_is_returned()
        {
            var clientId = Guid.NewGuid();
            Mediator.Send(Arg.Is<GetClientByIdRequest>(r => r.ClientId == clientId)).Returns(new Client() {ServiceDetails = new ServiceDetails()
            {
                PostPasswordResetReturnUrl = "https://returnurl", ServiceName = "Service Name"
            }});

            var result = await Controller.PasswordResetSuccessful(clientId);

            result.Should().BeOfType<ViewResult>();
            result.As<ViewResult>().ViewName.Should().Be("PasswordResetSuccessful");
            result.As<ViewResult>().Model.Should().BeOfType<PasswordResetSuccessfulViewModel>();
            result.As<ViewResult>().Model.As<PasswordResetSuccessfulViewModel>().ServiceName.Should().Be("Service Name");
            result.As<ViewResult>().Model.As<PasswordResetSuccessfulViewModel>().ReturnUrl.Should().Be("https://returnurl");
        }
    }
}