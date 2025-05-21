using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Template.Common.Captcha;
using Template.Common;
using Template.Shared.Services.Interfaces;

namespace Template.Shared.Services.Services
{
    public class CaptchaService : ICaptchaService
    {
        public string GetCaptcha(HttpContext context, bool noisy)
        {
            return Captcha.GetCaptcha(context, noisy);
        }

        public CaptchaDto VerifyCaptcha(HttpContext context, string captcha)
        {
            return Captcha.VerifyCaptcha(captcha, context);
        }
    }
}
