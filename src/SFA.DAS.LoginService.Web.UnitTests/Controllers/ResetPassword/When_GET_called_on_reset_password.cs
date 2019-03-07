using System;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.ResetPassword;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.ResetPassword
{
    [TestFixture]
    public class ResetPasswordControllerTestBase
    {
        protected ResetPasswordController Controller;
        protected Guid ClientId;
        protected IMediator Mediator;

        [SetUp]
        public void Arrange()
        {
            Mediator = Substitute.For<IMediator>();
            Controller = new ResetPasswordController(Mediator);
            ClientId = Guid.NewGuid();
        }
    }
    
    public class When_POST_called_on_reset_password : ResetPasswordControllerTestBase
    {
        [Test]
        public void Then_Request_is_passed_on_to_mediator()
        {
            var result = Controller.Post(ClientId, new ResetPasswordViewModel{Email = "forgot@password.com"});
            Mediator.Received().Send(Arg.Is<ResetPasswordCodeRequest>(r => r.Email == "forgot@password.com" && r.ClientId == ClientId));
        }
    }
    
    public class When_GET_called_on_reset_password : ResetPasswordControllerTestBase
    {
        private IActionResult _result;

        [SetUp]
        public async Task Act()
        {       
            _result = await Controller.Get(ClientId.ToString());
        }       
        
        [Test]
        public void Then_ViewResult_is_returned()
        {
            _result.Should().BeOfType<ViewResult>();
        }
        
        [Test]
        public void Then_ViewResult_is_returned_with_correct_view()
        {
            ((ViewResult) _result).ViewName.Should().Be("ResetPassword");
        }
        
        [Test]
        public void Then_ViewResult_is_returned_with_correct_viewmodel()
        {
            ((ViewResult) _result).Model.Should().BeOfType<ResetPasswordViewModel>();
            ((ResetPasswordViewModel) ((ViewResult) _result).Model).ClientId.Should().Be(ClientId);
        }
    }
}