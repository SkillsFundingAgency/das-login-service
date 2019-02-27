using System.Threading.Tasks;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.Interfaces
{
    public interface ICallbackService
    {
        Task Callback(Invitation sourceId, string loginUserId);
    }
}