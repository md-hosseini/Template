using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Template.API.Model.Dtos;
using Template.API.Model.RequestModels;
using Template.Common;
using Template.Shared.Services.Interfaces;

namespace Template.Shared.Services.Services
{
    public class SMSService : ISMSService
    {
        public async Task<SendSMSResponseDto> SendSMS(SendSMSRequestModel request)
        {
            var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.PostAsJsonAsync(AppSettingFactory.AppSetting.OTPSettings.Url, request);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadFromJsonAsync<SmsApiResult>();
                    return new SendSMSResponseDto
                    {
                        Response = json.Response.Response,
                        Status = json.Response.Status,
                        IsSucceeded = Convert.ToBoolean(json.Response.IsSucceeded)
                    };
                }
                else
                {
                    return new SendSMSResponseDto
                    {
                        Response = "BadRequest",
                        Status = (int)response.StatusCode,
                        IsSucceeded = false
                    };
                }
            }
            catch (Exception ex)
            {
                return new SendSMSResponseDto
                {
                    Response = "Exception",
                    Status = -1,
                    IsSucceeded = false
                };
            }
        }
    }
}
