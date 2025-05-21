using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Service.Interfaces.Services;
using Template.Service.Services.Services;
using Template.Shared.Services.Interfaces;
using Template.Shared.Services.Services;

namespace Template.Service
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceLayerServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ISeedDatabase, SeedDatabase>();
            return services;
        }
    }
}
