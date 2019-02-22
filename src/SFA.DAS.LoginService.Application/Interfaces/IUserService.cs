using System.Threading.Tasks;

namespace SFA.DAS.LoginService.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> UserExists(string email);
    }
}