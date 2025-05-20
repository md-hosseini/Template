using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Template.Common.Captcha;

namespace Template.Shared.Services.Interfaces
{
    public interface ICaptchaService
    {
        string GetCaptcha(HttpContext context, bool noisy);
        CaptchaDto VerifyCaptcha(HttpContext context, string captcha);
    }
}
