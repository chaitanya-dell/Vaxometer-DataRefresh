using System;
using System.Collections.Generic;
using System.Text;
using Vaxometer.Servicebus.Publishers;
using Vaxometer.Servicebus.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Vaxometer.Servicebus.Subscribers.BackgroundServices;
using Vaxometer.Servicebus.Subscribers;

namespace Vaxometer.Servicebus.IocExtension
{
    public static class IServiceExtension
    {
        public static IServiceCollection AddServicebus(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.Configure<ServicebusSettings>(configuration.GetSection(nameof(ServicebusSettings)));
            services.AddSingleton<IServicebusSettings>(x => x.GetRequiredService<IOptions<ServicebusSettings>>().Value);
            services.AddScoped<IMessagePublisher, MessagePublisher>();
            services.AddScoped<IChangeStreamSubscriber, ChangeStreamSubscriber>();
            services.AddHostedService<ChangeStreamSubscriberService>();

            return services;
        }
    }
}
