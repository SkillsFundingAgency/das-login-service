using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.CreatePassword
{
    public class When_invitation_resent : CreatePasswordTestsBase
    {
        private Guid _invitationId;
        
        [SetUp]
        public void Arrange()
        {
            _invitationId = Guid.NewGuid();
            
            Mediator.Send(Arg.Is<GetInvitationByIdRequest>(r => r.InvitationId == _invitationId), CancellationToken.None)
                .Returns(new Invitation() {Id = _invitationId, Email = "email@address.com"});
        }
        
        [Test]
        public async Task Then_mediator_is_asked_for_invitation()
        {
            await Controller.Reinvited(_invitationId);

            await Mediator.Received().Send(Arg.Is<GetInvitationByIdRequest>(r => r.InvitationId == _invitationId),
                CancellationToken.None);
        }
        
        [Test]
        public async Task Then_correct_ViewResult_is_returned()
        {         
            var result = await Controller.Reinvited(_invitationId);

            await Mediator.Received().Send(Arg.Is<GetInvitationByIdRequest>(r => r.InvitationId == _invitationId),
                CancellationToken.None);

            result.Should().BeOfType<ViewResult>();
            ((ViewResult) result).ViewName.Should().Be("Reinvited");
            ((ViewResult) result).Model.Should().BeOfType<ReinvitedViewModel>();
            ((ReinvitedViewModel) ((ViewResult) result).Model).Email.Should().Be("email@address.com");
        }
    }
}