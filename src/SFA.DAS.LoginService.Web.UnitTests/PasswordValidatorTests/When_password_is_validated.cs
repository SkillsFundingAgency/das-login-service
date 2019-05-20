using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Web.Infrastructure;
using SFA.DAS.LoginService.Web.UnitTests.Controllers.PasswordReset;

namespace SFA.DAS.LoginService.Web.UnitTests.PasswordValidatorTests
{
    [TestFixture]
    public class When_password_is_validated
    {
        private LoginContext _loginContext;
        private CustomPasswordValidator<LoginUser> _validator;
        private UserManager<LoginUser> _userManager;

        [SetUp]
        public void SetUp()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LoginContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _loginContext = new LoginContext(dbContextOptions);

            _validator = new CustomPasswordValidator<LoginUser>(Substitute.For<ILogger<CustomPasswordValidator<LoginUser>>>(), _loginContext);

            _userManager = new UserManager<LoginUser>(Substitute.For<IUserStore<LoginUser>>(), Substitute.For<IOptions<IdentityOptions>>(), Substitute.For<IPasswordHasher<LoginUser>>()
                , Substitute.For<IEnumerable<IUserValidator<LoginUser>>>(), new List<IPasswordValidator<LoginUser>>(), Substitute.For<ILookupNormalizer>(), new IdentityErrorDescriber(), Substitute.For<IServiceProvider>(),
                Substitute.For<ILogger<UserManager<LoginUser>>>());
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("\t", false)]
        [TestCase("\u0009", false)] // Unicode for TAB
        [TestCase("\u00A0", false)] // Unicode for non-breaking space
        [TestCase("       ", false)] // 8 spaces for whitespace check
        [TestCase("1234567", false)] // 7 digits
        [TestCase("abcdefg", false)] // 7 letters
        [TestCase("1234abc", false)] // 7 with only digits & letters
        [TestCase("123 abc", false)] // 7 with a mixture
        [TestCase("12345678", false)] // 8 digits
        [TestCase("abcdefgh", false)] // 8 letters
        [TestCase("11aaa222", true)]
        [TestCase(" 11AAA 222 ", true)]
        [TestCase("11AAA222!!", true)]
        public async Task Then_password_is_validated_against_rules_correctly(string newpassword, bool expectedValidity)
        {                       
            var result = await _validator.ValidateAsync(_userManager, new LoginUser(), newpassword);

            (result == IdentityResult.Success).Should().Be(expectedValidity);
        }

        [TestCase("1357Test", false)]
        [TestCase("1357Testing", false)]
        [TestCase("1357testinG", false)]
        [TestCase("1357TestinG", false)]
        [TestCase("1357TesTING", false)]
        [TestCase("1357TESting", false)]
        [TestCase("1357TESTING", false)]
        [TestCase("1357Testabc@~#123", false)]
        [TestCase("a1357Test", true)]
        [TestCase("a1357Testa", true)]
        public async Task Then_password_is_validated_against_blacklist_correctly(string newpassword, bool expectedValidity)
        {
            _loginContext.InvalidPasswords.Add(new InvalidPassword() { Password = "1357Test" });
            await _loginContext.SaveChangesAsync();

            var result = await _validator.ValidateAsync(_userManager, new LoginUser(), newpassword);

            (result == IdentityResult.Success).Should().Be(expectedValidity);
        }
    }
}