using System;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.CreatePassword
{
    public class When_GET_CreatePassword_with_non_existant_Invitation : CreatePasswordTestsBase
    {
        [Test]
        public void Then_BadRequest_Is_Returned()
        {
            Mediator.Send(Arg.Any<GetInvitationByIdRequest>()).Returns(default(Invitation));
            
            var result = Controller.Get(Guid.NewGuid()).Result;
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}