using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SFA.DAS.LoginService.Web.Infrastructure
{
    public class CustomPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : class
    {
        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            if (password.Length < 8 || 
                password.All(char.IsDigit) || 
                password.All(char.IsLetter) || 
                password.All(p => p == ' '))
            {
                return Task.FromResult(IdentityResult.Failed(new IdentityError(){Code = "PasswordValidity", Description = "Password does not meet validity rules"}));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}