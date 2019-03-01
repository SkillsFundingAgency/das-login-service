using FluentAssertions;
using MediatR;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.BuildLoginViewModel;

namespace SFA.DAS.LoginService.Application.UnitTests.Login.BuildLoginViewModel
{
    [TestFixture]
    public class BuildLoginViewModel_Types_Tests
    {
        [Test]
        public void BuildLoginViewModel_implements_irequest()
        {
            typeof(BuildLoginViewModelRequest).Should().Implement<IRequest<LoginViewModel>>();
        }
    }
}