using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Common
{
    public class AppSettingFactory
    {
        private static IConfiguration _configuration;
        public static AppSetting AppSetting { get; private set; }
        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
            AppSetting = new AppSetting
            {
                SqlConnection = _configuration["ConnectionStrings:ConnectionStr"],
                JwtSettings = new JwtSettings
                {
                    Key = _configuration["Jwt:Key"],
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"],
                    ExpiryInMinutes = int.Parse(_configuration["Jwt:ExpiryInMinutes"])
                },
            };
        }
    }

    public class AppSetting
    {
        public string SqlConnection { get; set; }
        public JwtSettings JwtSettings { get; set; }
    }
    public class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryInMinutes { get; set; }
    }
}
