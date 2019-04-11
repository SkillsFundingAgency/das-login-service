using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LoginService.Web.Controllers.CreateAccount.ViewModels;

namespace SFA.DAS.LoginService.Web.UnitTests.Controllers.CreateAccount
{
    [TestFixture]
    public class When_Validate_CreateAccountViewModel_with_missing_email
    {
        private CreateAccountViewModel _viewModel;
        private ValidationContext _context;
        private List<ValidationResult> _results;

        [SetUp]
        public void SetUp()
        {
            _viewModel = new CreateAccountViewModel();
            _context = new ValidationContext(_viewModel, null, null);
            _results = new List<ValidationResult>();
        }

        [Test]
        public void Then_Validation_Result_contains_Email_required_error()
        {
            _viewModel.GivenName = "Brian";
            _viewModel.FamilyName = "Smith";
            _viewModel.Username = string.Empty;
            _viewModel.Password = "AValidPassw0rd1";
            _viewModel.ConfirmPassword = "AValidPassw0rd1";

            var isModelStateValid = Validator.TryValidateObject(_viewModel, _context, _results, true);

            isModelStateValid.Should().BeFalse();
            _results.Count.Should().Be(1);
            _results[0].ErrorMessage.Should().Be("Enter a email address");
            _results[0].MemberNames.Should().OnlyContain(p => p == nameof(CreateAccountViewModel.Username));
        }
    }
}