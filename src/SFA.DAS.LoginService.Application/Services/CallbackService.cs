using System.Net.Http;
using Newtonsoft.Json;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.Services
{
    public class CallbackService : ICallbackService
    {
        private readonly HttpClient _httpClient;

        public CallbackService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void Callback(Invitation invitation, string loginUserId)
        {
            _httpClient.PostAsync(invitation.CallbackUri, new StringContent(JsonConvert.SerializeObject(new {sub=loginUserId, sourceId = invitation.SourceId})));
        }
    }
}