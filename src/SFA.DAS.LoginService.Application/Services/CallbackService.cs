using System;
using System.Net.Http;
using System.Text;
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
            var userLog = new UserLog()
            {
                Id = GuidGenerator.NewGuid(), 
                Action = "Client callback", 
                Email = invitation.Email,  
                DateTime = SystemTime.UtcNow()
            };
            _loginContext.UserLogs.Add(userLog);
            
            HttpResponseMessage response;
            try
            {
                
                response = await _httpClient.PostAsync(invitation.CallbackUri, 
                    new StringContent(JsonConvert.SerializeObject(
                        new
                        {
                            sub=loginUserId, 
                            sourceId = invitation.SourceId,
                            inviterId = invitation.InviterId
                        }), Encoding.UTF8, "application/json"));
            }
            catch (HttpRequestException ex)
            {
                userLog.Result = "Callback error";
                userLog.ExtraData = JsonConvert.SerializeObject(new
                {
                    CallbackUri = invitation.CallbackUri, 
                    SourceId = invitation.SourceId, 
                    UserId = loginUserId,  
                    Content = ex.Message
                });
                await _loginContext.SaveChangesAsync();
                return;
            }

            
            
            if (response.IsSuccessStatusCode)
            {
                invitation.IsCalledBack = true;
                invitation.CallbackDate = SystemTime.UtcNow();

                userLog.Result = "Callback OK";
                userLog.ExtraData = JsonConvert.SerializeObject(new {CallbackUri = invitation.CallbackUri, SourceId = invitation.SourceId, UserId = loginUserId});
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                
                userLog.Result = "Callback error";
                userLog.ExtraData = JsonConvert.SerializeObject(new
                {
                    CallbackUri = invitation.CallbackUri, 
                    SourceId = invitation.SourceId, 
                    UserId = loginUserId, 
                    ResponseStatusCode = response.StatusCode, 
                    Content = content
                });
            }
            await _loginContext.SaveChangesAsync();
            
        }
    }
}