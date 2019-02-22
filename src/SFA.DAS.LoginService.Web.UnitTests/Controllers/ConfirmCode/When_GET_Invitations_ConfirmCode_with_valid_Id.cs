using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.LoginService.Web.Controllers.InvitationWeb;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.ConfirmCode
{
    [TestFixture]
    public class When_GET_Invitations_ConfirmCode_with_valid_Id
    {
        [Test]
        public void Then_ViewResult_Is_Returned()
        {
            var controller = new ConfirmCodeController();
            var result = controller.Get(Guid.NewGuid());
            result.Should().BeOfType<ViewResult>();
        }

//        [Test]
//        public void Then_ViewResult_view_is_ConfirmCode_cshtml()
//        {
//            var controller = new ConfirmCodeController();
//            var result = controller.Get(Guid.NewGuid());
//            ((ViewResult) result).ViewName.Should().Be("ConfirmCode");
//        }
    }
}