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
using SFA.DAS.LoginService.Data.Entities;
using SFA.DAS.LoginService.Web.Infrastructure;

namespace SFA.DAS.LoginService.Web.UnitTests.PasswordValidatorTests
{
    [TestFixture]
    public class When_password_is_verified
    {
        [TestCase("password1", PasswordVerificationResult.Success, "AQAAAAEAACcQAAAAEK9g +2/kzLtsxB7lpShiO1DkoS+Ivs1fLn47rBaIWaTYDgAScQkt3yaLhTa6FBv9og==")]
        [TestCase("Password456", PasswordVerificationResult.SuccessRehashNeeded, "/0pZT8rmWAWxpZFvWqKW15WjL0QszX3nYMLdJ7Z0rJCTXESXuwlZ0uV+7vR7hGKBsJZD3cjJB31zEE3JNymjvdk1DcPdIRa49PEG9PFejMFuRjdOo4CQUvwnNxBsV6WRONBUwbrgo839BtNrYDum6m+3IZGvHBLfR87hU+M4jPGwKTTs9iiNZMUNP+Lrpe+MLWZLnowghy62r7NWeuhrG5bQeH5+RVMeUif8me9lymyyptl1bcEGptnR/Z87eorF6mzVPVjSz+3UOnPS3m1uhm7oPxsYubYhFGGlGFcUHEho4Q7rrkPQ1oXSeplWW14w1f3qJ5n05uaMfNl6S5buzmQIu+S7mZP++lVCDNchTDmU")]
        [TestCase("Notthepassword", PasswordVerificationResult.Failed, "/0pZT8rmWAWxpZFvWqKW15WjL0QszX3nYMLdJ7Z0rJCTXESXuwlZ0uV+7vR7hGKBsJZD3cjJB31zEE3JNymjvdk1DcPdIRa49PEG9PFejMFuRjdOo4CQUvwnNxBsV6WRONBUwbrgo839BtNrYDum6m+3IZGvHBLfR87hU+M4jPGwKTTs9iiNZMUNP+Lrpe+MLWZLnowghy62r7NWeuhrG5bQeH5+RVMeUif8me9lymyyptl1bcEGptnR/Z87eorF6mzVPVjSz+3UOnPS3m1uhm7oPxsYubYhFGGlGFcUHEho4Q7rrkPQ1oXSeplWW14w1f3qJ5n05uaMfNl6S5buzmQIu+S7mZP++lVCDNchTDmU")]
        [TestCase("password1", PasswordVerificationResult.Success, "AQAAAAEAACcQAAAAEIswd9FaI6vvn/OfF2L1LL1HRGE//XcbGkhWu+hKy83UW3sIWw/yetRsbGuh8qZ+Jg==")]
        [TestCase("Notthepassword", PasswordVerificationResult.Failed, "AQAAAAEAACcQAAAAEIswd9FaI6vvn/OfF2L1LL1HRGE//XcbGkhWu+hKy83UW3sIWw/yetRsbGuh8qZ+Jg==")]
        [TestCase("Password123", PasswordVerificationResult.Success, "AQAAAAEAACcQAAAAEO384EEFSS1MWY8C8MTTjxZaWnK/7jHBgT5oG9k+EBv5N+jMQITA4K/SqQKDTZ/Rgg==")]
        [TestCase("Password789", PasswordVerificationResult.Success, "AQAAAAEAACcQAAAAENH1Std7x7rh2GLqnUsysknUiZZ1N3mZwvh/RI47y4U3q/foLs7U0bZ0xqA92oD6tA==")]
        [TestCase("Windmill1", PasswordVerificationResult.Success, "AQAAAAEAACcQAAAAEOe9xLvzaymVSfgVxAr8ziMAN1Fqfo2H2GdP0a5spHnnP/44Vugz/8JM2RjnFdfJJQ==")]
        [TestCase("Notthepassword", PasswordVerificationResult.Failed, "/52lvW+iinRlZCJmdHP9jap1zChI8BOxqn2tUYBXvh7nAJr5Afi8HPweCVp324EpqJC36dki3aAw4m61RF84dOSmSSbUWKCZMGZtSD6yCaXV+FVVBeTcrjOqH5qnmZ1rnBzVcS16AxhMQjHHwiPQ5h4IvNyiQLqqusNoxkCYTGbv4eq6ky0d+/8Vmfb1RRDBkHIJABqFPct3Fi/917A0VVIko3+4y5GaupfPxq+5r7Bb61c+Y1b8BMAUfFqFjcsP3TBhGAb0jSby96YLVZVOpM47VoXLuAWxkgZRyvWdmxY0IC/dqIKmIvdgrnsLn5zSEqNTmx5hXl72bOJGvRqzLL/nd316ACDMULU1VZ5/Vk+Y")]
        public void Then_password_is_verified_correctly(string providedPassword, PasswordVerificationResult expectedResult, string hashedPassword)
        {
            var passwordHasher = new LoginServicePasswordHasher<LoginUser>();

            var result = passwordHasher.VerifyHashedPassword(new LoginUser(), hashedPassword, providedPassword);

            result.Should().Be(expectedResult);
        }
    }
}