using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<LoginUser> _userManager;

        public UserService(UserManager<LoginUser> userManager)
        {
            _userManager = userManager;
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
    }
}