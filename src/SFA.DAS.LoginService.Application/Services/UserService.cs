using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<LoginUser> _userManager;
        private readonly CustomSignInManager _signInManager;

        public UserService(UserManager<LoginUser> userManager, CustomSignInManager signInManager)
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

        public async Task LockoutUser(string email)
        {
            var user = await FindByEmail(email);
            user.IsEnabled = false;
            await _userManager.UpdateAsync(user);
        }

        public async Task<UserResponse> ResetPassword(string email, string password, string identityToken)
        {
            var user = await FindByEmail(email);
            var identityResult = await _userManager.ResetPasswordAsync(user, identityToken, password);
            user.IsEnabled = true;
            await _userManager.UpdateAsync(user);
            await _userManager.ResetAccessFailedCountAsync(user);
            
            return new UserResponse(){Result = identityResult, User = user};
        }

        public async Task<string> GeneratePasswordResetToken(LoginUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
    }
}