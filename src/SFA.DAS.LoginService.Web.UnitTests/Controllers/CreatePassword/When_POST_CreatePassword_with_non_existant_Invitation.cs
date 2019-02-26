using System;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.CreatePassword
{
    [TestFixture]
    public class When_POST_CreatePassword_with_non_existant_Invitation
    {
        [Test]
        public void Then_BadRequest_Is_Returned()
        {
            var mediator = Substitute.For<IMediator>();
            mediator.Send(Arg.Any<GetInvitationByIdRequest>()).Returns(default(InvitationResponse));
            
            var controller = new CreatePasswordController(mediator);
            var result = controller.Post(new CreatePasswordViewModel(){InvitationId = Guid.NewGuid()}).Result;
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}