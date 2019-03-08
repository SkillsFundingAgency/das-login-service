using MediatR;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.PasswordReset
{
    [TestFixture]
    public class PasswordResetTestBase
    {
        protected IMediator Mediator;
        protected ResetPasswordController Controller;
        
        [SetUp]
        public void SetUp()
        {
            Mediator = Substitute.For<IMediator>();
            Controller = new ResetPasswordController(Mediator);
        }
    }
}