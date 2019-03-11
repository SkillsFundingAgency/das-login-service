using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.SignUpComplete
{
    [TestFixture]
    public class When_GET_SignUpComplete_for_non_existant_invitation : SignUpCompleteTestsBase
    {
        [Test]
        public async Task Then_BadRequest_is_returned()
        {
            Mediator.Send(Arg.Any<GetInvitationByIdRequest>(), CancellationToken.None).Returns(default(Invitation));
            var result = await Controller.Get(InvitationId);

            result.Should().BeOfType<BadRequestResult>();
        }
    }
}