using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.Services
{
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

        public async Task<CreateUserResponse> CreateUser(LoginUser newUser, string password)
        {
            var result = await _userManager.CreateAsync(newUser, password);
            return new CreateUserResponse {Result = result, User = newUser};
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
    }
}