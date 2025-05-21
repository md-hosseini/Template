using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.API.Model.Dtos;
using Template.API.Model.RequestModels;

namespace Template.Shared.Services.Interfaces
{
    public interface ISMSService
    {
        Task<SendSMSResponseDto> SendSMS(SendSMSRequestModel request);
    }
}
