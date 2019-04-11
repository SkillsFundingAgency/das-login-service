using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Web.Controllers.CreateAccount.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.CreateAccount
{
    public class When_GET_CreateAccount : CreateAccountTestBase
    {
        private string _returnUrl = Uri.EscapeDataString("http://returnhere.com");

        [Test]
        public void Then_correct_ViewResult_is_returned()
        {
            var result = Controller.Get(_returnUrl);
            result.Should().BeOfType<ViewResult>();
            ((ViewResult) result).ViewName.Should().Be("CreateAccount");
        }
        
        [Test]
        public void Then_correct_CreatePasswordViewModel_is_passed_to_View()
        {
            var result = Controller.Get(_returnUrl);

            ((ViewResult) result).Model.Should().BeOfType<CreateAccountViewModel>();
            ((CreateAccountViewModel)((ViewResult)result).Model).ReturnUrl.Should().Be(Uri.UnescapeDataString(_returnUrl));
        }
    }
}