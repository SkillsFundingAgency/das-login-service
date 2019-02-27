using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetInvitationById;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.SignUpComplete
{
    [TestFixture]
    public class When_GET_SignUpComplete_for_non_existant_invitation : SignUpCompleteTestsBase
    {
        [Test]
        public async Task Then_BadRequest_is_returned()
        {
            Mediator.Send(Arg.Any<GetInvitationByIdRequest>(), CancellationToken.None).Returns(default(InvitationResponse));
            var result = await Controller.Get(InvitationId);

            result.Should().BeOfType<BadRequestResult>();
        }
    }
    
    [TestFixture]
    public class When_GET_SignUpComplete_for_invitation_with_unconfirmed_code : SignUpCompleteTestsBase
    {
        [Test]
        public async Task Then_RedirectResult_to_ConfirmCode_is_returned()
        {
            SetUnconfirmedValidInvitationByIdRequest();
            var result = await Controller.Get(InvitationId);

            result.Should().BeOfType<RedirectToActionResult>();
            ((RedirectToActionResult) result).ActionName.Should().Be("Get");
            ((RedirectToActionResult) result).ControllerName.Should().Be("ConfirmCode");
        }
    }
}