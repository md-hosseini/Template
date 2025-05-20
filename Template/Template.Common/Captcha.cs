using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Template.Common
{
    public static class Captcha
    {
        public static CaptchaDto VerifyCaptcha(string captcha, HttpContext context)
        {
            if (string.IsNullOrWhiteSpace(captcha))
                return new CaptchaDto
                {
                    IsValid = false,
                    Response = new ApiResponseDto()
                    {
                        Status = "",
                        StatusCode = ApiResponseStatusCode.Error,
                        StatusDesc = "Enter Code."
                    }
                };

            if (context.Session.GetString("Captcha") == null || captcha != context.Session.GetString("Captcha"))
                return new CaptchaDto
                {
                    IsValid = false,
                    Response = new ApiResponseDto()
                    {
                        Status = "",
                        StatusCode = ApiResponseStatusCode.Error,
                        StatusDesc = "Entered code is wrong."
                    }
                };

            ClearCaptcha(context);

            return new CaptchaDto { IsValid = true };
        }

        internal static void ClearCaptcha(HttpContext context)
        {
            if (context.Session.GetString("Captcha") != null)
                context.Session.Remove("Captcha");
        }



        public static string GetCaptcha(HttpContext context, bool noisy = true)
        {
            // ایجاد یک نمونه از RandomNumberGenerator
            using (var rng = RandomNumberGenerator.Create())
            {
                // تولید اعداد تصادفی برای سوال کپچا
                int a = GetRandomNumber(rng, 10, 99);
                int b = GetRandomNumber(rng, 1, 9);
                var captcha = $"{a} + {b} = ?";
                var captchaValue = (a + b).ToString();

                // ایجاد تصویر
                using (var mem = new MemoryStream())
                using (var bmp = new Bitmap(150, 35))
                using (var gfx = Graphics.FromImage(bmp))
                {
                    gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                    gfx.SmoothingMode = SmoothingMode.AntiAlias;
                    gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                    // افزودن نویز
                    if (noisy)
                    {
                        AddNoise(gfx, rng, bmp.Width, bmp.Height);
                    }

                    // افزودن سوال کپچا
                    gfx.DrawString(captcha, new System.Drawing.Font("Tahoma", 18), Brushes.Gray, 2, 3);

                    // تبدیل تصویر به فرمت Base64
                    bmp.Save(mem, ImageFormat.Jpeg);
                    var imageBase64 = Convert.ToBase64String(mem.ToArray());

                    // ذخیره مقدار کپچا در سشن
                    context.Session.SetString("Captcha", captchaValue);

                    return imageBase64;
                }
            }
        }

        private static int GetRandomNumber(RandomNumberGenerator rng, int minValue, int maxValue)
        {
            // تولید یک عدد تصادفی بین minValue و maxValue
            var diff = maxValue - minValue;
            var uint32Buffer = new byte[4];
            int randomValue;
            do
            {
                rng.GetBytes(uint32Buffer);
                randomValue = BitConverter.ToInt32(uint32Buffer, 0) & int.MaxValue;
            } while (randomValue >= diff * (int.MaxValue / diff));
            return minValue + (randomValue % diff);
        }

        private static void AddNoise(Graphics gfx, RandomNumberGenerator rng, int width, int height)
        {
            // افزودن نویز به تصویر
            var pen = new Pen(Color.Yellow);
            for (int i = 0; i < 10; i++)
            {
                pen.Color = Color.FromArgb(
                    GetRandomNumber(rng, 0, 256),
                    GetRandomNumber(rng, 0, 256),
                    GetRandomNumber(rng, 0, 256));

                int r = GetRandomNumber(rng, 0, width / 3);
                int x = GetRandomNumber(rng, 0, width);
                int y = GetRandomNumber(rng, 0, height);

                gfx.DrawEllipse(pen, x - r, y - r, r, r);
            }
        }




        public enum ApiResponseStatusCode
        {
            [Description("Done")]
            OK = 200,
            [Description("Failed")]
            Error = 505,
        }

        public class ApiResponseDto
        {
            public string Status { get; set; }
            public ApiResponseStatusCode StatusCode { get; set; }
            public string StatusDesc { get; set; }
        }
        public class CaptchaDto
        {
            public bool IsValid { get; set; }
            public ApiResponseDto Response { get; set; }
        }
    }
}
