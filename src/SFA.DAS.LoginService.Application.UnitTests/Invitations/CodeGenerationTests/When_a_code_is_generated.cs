using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LoginService.Application.Services;

namespace SFA.DAS.LoginService.Application.UnitTests.Invitations.CodeGenerationTests
{
    [TestFixture]
    public class When_a_code_is_generated
    {
        [Test]
        public void Then_a_code_of_the_correct_length_is_returned()
        {
            var codeGenerationService = new CodeGenerationService();

            var result = codeGenerationService.GenerateCode();

            result.Length.Should().Be(8);
        }
        
        [Test]
        public void Then_a_code_of_Try_again_is_not_returned()
        {
            var codeGenerationService = new CodeGenerationService();

            var result = codeGenerationService.GenerateCode();

            result.Should().NotBe("Try again");
        }
        
        [Test]
        public void Then_a_code_only_containing_uppercase_letters_is_returned()
        {
            var codeGenerationService = new CodeGenerationService();

            var result = codeGenerationService.GenerateCode();

            var numberOfLettersInCode = result.Count(char.IsLetter);
            var numberOfUppercaseLettersInCode = result.Count(char.IsUpper);
            var numberOfLowerCaseLettersInCode = result.Count(char.IsLower);
            
            numberOfUppercaseLettersInCode.Should().Be(numberOfLettersInCode);
            numberOfLowerCaseLettersInCode.Should().Be(0);
        }
    }
}