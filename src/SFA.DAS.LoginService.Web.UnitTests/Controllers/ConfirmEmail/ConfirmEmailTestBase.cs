using System;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Web.Controllers.ConfirmEmail;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.ConfirmEmail
{
    [TestFixture]
    public class ConfirmEmailTestBase
    {
        protected IMediator Mediator;
        protected ConfirmEmailController Controller;

        protected readonly string ReturnUrl = Uri.EscapeDataString("http://localhost/returnurl");
        protected readonly string IdentityToken = Uri.EscapeDataString("1234567890+ABCDEF/GHIJKLMN/OPQRS+TUVXYZ==");

        [SetUp]
        public void SetUp()
        {
            Mediator = Substitute.For<IMediator>();
            Controller = new ConfirmEmailController(Mediator);
        }
    }
}