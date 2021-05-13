using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Vaxometer.Servicebus.Settings;

namespace Vaxometer.Servicebus.IocExtension
{
    public static class IServiceExtension
    {
        public static IServiceCollection AddServicebus(IServiceCollection services, IConfiguration configuration)
        {
            
            //services.Configure<ServicebusSettings>(configuration.GetSection(nameof(ServicebusSettings)));
            services.AddSingleton<IServicebusSettings>(x => x.GetRequiredService<IOptions<ServicebusSettings>>().Value);


            return services;
        }
    }
}
