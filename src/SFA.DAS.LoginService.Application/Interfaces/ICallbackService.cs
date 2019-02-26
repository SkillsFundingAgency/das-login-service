using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.Interfaces
{
    public interface ICallbackService
    {
        void Callback(Invitation sourceId, string loginUserId);
    }
}