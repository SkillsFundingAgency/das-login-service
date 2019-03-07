using System;
using MediatR;
using NSubstitute;
using NUnit.Framework;
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
}