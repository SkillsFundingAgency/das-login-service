using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.LoginService.Application.Interfaces;
using SFA.DAS.LoginService.Data;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.Services
{
    public class CallbackService : ICallbackService
    {
        private readonly HttpClient _httpClient;
        private readonly LoginContext _loginContext;

        public CallbackService(HttpClient httpClient, LoginContext loginContext)
        {
            _httpClient = httpClient;
            _loginContext = loginContext;
        }

        public async Task Callback(Invitation invitation, string loginUserId)
        {
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.PostAsync(invitation.CallbackUri, new StringContent(JsonConvert.SerializeObject(new {sub=loginUserId, sourceId = invitation.SourceId})));
            }
            catch (HttpRequestException)
            {
                return;
            }
            
            if (response.IsSuccessStatusCode)
            {
                invitation.IsCalledBack = true;
                invitation.CallbackDate = SystemTime.UtcNow();
                await _loginContext.SaveChangesAsync();
            }
        }
    }
}