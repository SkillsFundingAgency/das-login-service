using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
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
        
        public async Task<LoginUser> FindById(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
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

        public async Task AddUserClaim(LoginUser user, string claimType, string value)
        {
            await _userManager.AddClaimAsync(user, new Claim(claimType, value));
        }

        public async Task<UserResponse> ChangeEmail(Guid userId, string email)
        {
            var user = await FindById(userId);
            var identityResult = await _userManager.SetUserNameAsync(user, email);
            var emailResult = await _userManager.SetEmailAsync(user, email);
            await _userManager.UpdateAsync(user);

            return new UserResponse() { Result = identityResult, User = user };
        }

    }
}