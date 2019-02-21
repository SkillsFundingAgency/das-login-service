using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Application.Services;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.CodeGenerationTests
{
    [TestFixture]
    public class CodeGenerationService_Type_Tests
    {
        [Test]
        public void CodeGenerationService_implements_ICodeGenerationService()
        {
            typeof(CodeGenerationService).Should().Implement<ICodeGenerationService>();
        }
    }
}