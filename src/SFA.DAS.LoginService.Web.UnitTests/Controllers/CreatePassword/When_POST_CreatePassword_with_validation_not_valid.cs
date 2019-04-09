using System;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb;
using SFA.DAS.LoginService.Web.Controllers.InvitationsWeb.ViewModels;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.CreatePassword
{
    [TestFixture]
    public class When_POST_CreatePassword_with_validation_not_valid
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
            _controller.ModelState.AddModelError("", "");
        }

        [TestCase("", "")]
        [TestCase("password", "")]
        [TestCase("", "password")]
        public async Task Then_View_is_returned_with_identical_ViewModel(string password, string confirmPassword)
        {
            var createPasswordViewModel = new CreatePasswordViewModel() { Password = password, ConfirmPassword = confirmPassword };
            var result = await _controller.Post(createPasswordViewModel);
            result.Should().BeOfType<ViewResult>();
            ((ViewResult) result).ViewName.Should().Be("CreatePassword");
            ((ViewResult) result).Model.Should().BeOfType<CreatePasswordViewModel>();
            ((CreatePasswordViewModel) ((ViewResult) result).Model).Should().BeSameAs(createPasswordViewModel);
        }
        
        
    }
}