using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LoginService.Web.Controllers.ResetPassword.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.CreatePassword
{
    [TestFixture]
    public class When_Validate_ResetPasswordViewModel_with_non_matching_passwords
    {
        private ResetPasswordViewModel _viewModel;
        private ValidationContext _context;
        private List<ValidationResult> _results;

        [SetUp]
        public void SetUp()
        {
            _viewModel = new ResetPasswordViewModel();
            _context = new ValidationContext(_viewModel, null, null);
            _results = new List<ValidationResult>();
        }
        
        [Test]
        public void Then_Validation_Result_contains_ConfirmPassword_non_matching_error()
        {
            _viewModel.Password = "Pa55word";
            _viewModel.ConfirmPassword = "Pa44word";
            
            var isModelStateValid = Validator.TryValidateObject(_viewModel, _context, _results, true);

            isModelStateValid.Should().BeFalse();
            _results.Count.Should().Be(1);
            _results[0].ErrorMessage.Should().Be("Passwords should match");

            // unfortunately the CompareAttribute does not correctly set the MemberNames so cannot explictly 
            // check which property would display this error on the view 
            //_results[0].MemberNames.Should().OnlyContain(p => p == nameof(CreatePasswordViewModel.ConfirmPassword));
        }
    }
}