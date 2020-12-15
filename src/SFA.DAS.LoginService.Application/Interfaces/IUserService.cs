using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> UserExists(string email);
        Task<UserResponse> CreateUser(LoginUser newUser, string password);
        Task<SignInResult> SignInUser(string username, string password, bool rememberLogin);
        Task<LoginUser> FindByUsername(string username);
        Task<LoginUser> FindById(Guid id);
        Task SignOutUser();
        Task<LoginUser> FindByEmail(string email);
        Task<UserResponse> ResetPassword(string email, string password, string identityToken);
        Task<string> GeneratePasswordResetToken(LoginUser user);
        Task AddUserClaim(LoginUser user, string claimType, string value);
        Task<UserResponse> ChangeEmail(Guid userId, string email);
    }

    public class UserResponse
    {
        public LoginUser User { get; set; }
        public IdentityResult Result { get; set; }
    }
}