using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.SignUpComplete
{
    [TestFixture]
    public class When_GET_SignUpComplete_for_valid_invitation : SignUpCompleteTestsBase
    {
        [Test]
        public async Task Then_mediator_is_asked_for_an_invitation()
        {
            SetValidInvitationByIdRequest();
            await Controller.Get(InvitationId);
            await Mediator.Received().Send(Arg.Is<GetInvitationByIdRequest>(r => r.InvitationId == InvitationId), CancellationToken.None);
        }

        [Test]
        public async Task Then_ViewResult_is_returned_with_correct_view_model()
        {            
            SetValidInvitationByIdRequest();
            var result = await Controller.Get(InvitationId);

            result.Should().BeOfType<ViewResult>();
            ((ViewResult) result).ViewName.Should().Be("SignUpComplete");
            ((ViewResult) result).Model.Should().BeOfType<SignUpCompleteViewModel>();
            ((SignUpCompleteViewModel) ((ViewResult) result).Model).UserRedirectUri.Should().Be(new Uri("https://localhost/redirect"));
        }
    }
}