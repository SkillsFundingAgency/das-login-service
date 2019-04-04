using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.Services
{
    [ExcludeFromCodeCoverage]
    public class UserService : IUserService
    {
        private readonly UserManager<LoginUser> _userManager;
        private readonly SignInManager<LoginUser> _signInManager;

        public UserService(UserManager<LoginUser> userManager, SignInManager<LoginUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> UserExists(string email)
        {
             return (await _userManager.FindByIdAsync(email)) != null;
        }

        public async Task<UserResponse> CreateUser(LoginUser newUser, string password)
        {
            var result = await _userManager.CreateAsync(newUser, password);
            return new UserResponse {Result = result, User = newUser};
        }

        public async Task<SignInResult> SignInUser(string username, string password, bool rememberLogin)
        {
            return await _signInManager.PasswordSignInAsync(username, password, rememberLogin, lockoutOnFailure: true);
        }

        public async Task<LoginUser> FindByUsername(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task SignOutUser()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<LoginUser> FindByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<UserResponse> ResetPassword(string email, string password, string identityToken)
        {
            var user = await FindByEmail(email);
            var identityResult = await _userManager.ResetPasswordAsync(user, identityToken, password);
            await _userManager.UpdateAsync(user);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
            
            return new UserResponse(){Result = identityResult, User = user};
        }

        public async Task<string> GeneratePasswordResetToken(LoginUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> VerifyConfirmEmailToken(LoginUser user, string identityToken)
        {
            return await _userManager.ConfirmEmailAsync(user, identityToken);
        }

        public async Task<string> GenerateConfirmEmailToken(LoginUser user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<bool> UserHasConfirmedEmail(LoginUser user)
        {
            return (await _userManager.IsEmailConfirmedAsync(user));
        }
    }
}