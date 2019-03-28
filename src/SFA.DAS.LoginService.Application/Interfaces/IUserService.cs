using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> UserExists(string email);

        /// <summary>
        /// Gets a flag which determines if the email for specified user has been verified.
        /// </summary>
        /// <param name="user">The user to check for email address verification.</param>
        /// <returns>true if the email address is verfied otherwise false</returns>
        Task<bool> UserHasConfirmedEmail(LoginUser user);

        Task<UserResponse> CreateUser(LoginUser newUser, string password);
        Task<SignInResult> SignInUser(string username, string password, bool rememberLogin);
        Task<LoginUser> FindByUsername(string username);
        Task SignOutUser();
        Task<LoginUser> FindByEmail(string email);
        Task<UserResponse> ResetPassword(string email, string password, string identityToken);
        Task<string> GeneratePasswordResetToken(LoginUser user);
    }

    public class UserResponse
    {
        public LoginUser User { get; set; }
        public IdentityResult Result { get; set; }
    }
}