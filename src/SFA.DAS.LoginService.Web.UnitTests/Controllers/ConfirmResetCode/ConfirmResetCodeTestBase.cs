using System;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.ConfirmResetCode
{
    [TestFixture]
    public class ConfirmResetCodeTestBase
    {
        protected ConfirmResetCodeController Controller;
        protected Guid RequestId;
        protected Guid ClientId;
        protected IMediator Mediator;

        [SetUp]
        public void SetUp()
        {
            Mediator = Substitute.For<IMediator>();
            
            Controller = new ConfirmResetCodeController(Mediator);
            RequestId = Guid.NewGuid();
            ClientId = Guid.NewGuid();
        }
    }
}