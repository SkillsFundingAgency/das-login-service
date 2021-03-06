using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Configuration;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.ResetPassword
{
    public class When_GET_called_on_CodeSent : RequestPasswordResetControllerTestBase
    {
        [Test]
        public void Then_ViewResult_is_returned()
        {
            var result = Controller.CodeSent(Guid.NewGuid(), "email@email.com");
            result.Should().BeOfType<ViewResult>();
            result.As<ViewResult>().ViewName.Should().Be("CodeSent");
            result.As<ViewResult>().Model.Should().BeOfType<CodeSentViewModel>();
            result.As<ViewResult>().Model.As<CodeSentViewModel>().Email.Should().Be("email@email.com");
        }
    }
}