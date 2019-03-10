using System;
using System.Linq;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.CreatePassword;
using SFA.DAS.LoginService.Application.GetInvitationById;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.CreatePassword
{
    [TestFixture]
    public class When_POST_CreatePassword_with_non_matching_passwords
    {
        private CreatePasswordController _controller;
        private Guid _invitationId;
        private IMediator _mediator;

        [SetUp]
        public void SetUp()
        {
            _mediator = Substitute.For<IMediator>();
            _controller = new CreatePasswordController(_mediator);
            _invitationId = Guid.NewGuid();
        }
        
        [Test]
        public void Then_mediator_is_not_called()
        {
            _controller.Post(new CreatePasswordViewModel() {InvitationId = _invitationId, PasswordViewModel = new PasswordViewModel{ Password = "Pa55word", ConfirmPassword = "P4ssword"}}).Wait();
            _mediator.DidNotReceiveWithAnyArgs().Send(Arg.Any<CreatePasswordRequest>());
        }

        [Test]
        public void Then_CreatePassword_ViewResult_is_returned()
        {
            _mediator.Send(Arg.Any<GetInvitationByIdRequest>()).Returns(new Invitation() {CodeConfirmed = true});
            var result = _controller.Post(new CreatePasswordViewModel() {InvitationId = _invitationId, PasswordViewModel = new PasswordViewModel{ Password = "Pa55word", ConfirmPassword = "P4ssword"}}).Result;

            result.Should().BeOfType<ViewResult>();
            ((ViewResult) result).ViewName.Should().Be("CreatePassword");
            
            _controller.ModelState.Count.Should().Be(1);
            _controller.ModelState.ValidationState.Should().Be(ModelValidationState.Invalid);
            _controller.ModelState.First().Key.Should().Be("Password");
            _controller.ModelState.First().Value.Errors.First().ErrorMessage.Should().Be("Passwords should match");
        }
        
        [Test]
        public void Then_CreatePassword_ViewResult_contains_CreatePasswordViewModel()
        {
            _mediator.Send(Arg.Any<GetInvitationByIdRequest>()).Returns(new Invitation() {CodeConfirmed = true});
            var result = _controller.Post(new CreatePasswordViewModel() {InvitationId = _invitationId, PasswordViewModel = new PasswordViewModel{ Password = "Pa55word", ConfirmPassword = "P4ssword"}}).Result;

            ((ViewResult) result).Model.Should().BeOfType<CreatePasswordViewModel>();
            ((CreatePasswordViewModel) ((ViewResult) result).Model).PasswordViewModel.Password.Should().Be("Pa55word");
            ((CreatePasswordViewModel) ((ViewResult) result).Model).PasswordViewModel.ConfirmPassword.Should().Be("P4ssword");
        }
    }
}