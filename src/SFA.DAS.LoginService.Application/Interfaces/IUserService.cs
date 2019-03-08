using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> UserExists(string email);
        Task<CreateUserResponse> CreateUser(LoginUser newUser, string password);
        Task<SignInResult> SignInUser(string username, string password, bool rememberLogin);
        Task<LoginUser> FindByUsername(string username);
        Task SignOutUser();
        Task<LoginUser> FindByEmail(string email);
        Task LockoutUser(string email);
    }
    
    public class CreateUserResponse
    {
        public LoginUser User { get; set; }
        public IdentityResult Result { get; set; }
    }
}