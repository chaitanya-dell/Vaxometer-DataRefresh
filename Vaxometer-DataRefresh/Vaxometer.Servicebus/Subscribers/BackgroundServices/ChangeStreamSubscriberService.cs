using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Vaxometer.Servicebus.Subscribers.BackgroundServices
{
    public class ChangeStreamSubscriberService : BackgroundService
    {
        public IServiceProvider _services { get; }
        private readonly ILogger<ChangeStreamSubscriberService> _logger;


        public ChangeStreamSubscriberService(IServiceProvider services, ILoggerFactory logger)
        {
            _services = services;
            _logger = logger.CreateLogger<ChangeStreamSubscriberService>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped ChangeStream subscriber Service Hosted Service running.");

            try
            {
                using (var scope = _services.CreateScope())
                {
                    var subscriber =
                        scope.ServiceProvider
                            .GetRequiredService<IChangeStreamSubscriber>();
                    
                    subscriber.RegisterOnMessageHandlerAndReceiveMessages();
                }
                await Task.Delay(10, stoppingToken);
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message, ex);
            }
        }
    }
}
