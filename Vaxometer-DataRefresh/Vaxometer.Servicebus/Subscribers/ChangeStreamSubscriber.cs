using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vaxometer.Servicebus.Messages;
using Vaxometer.Servicebus.Settings;

namespace Vaxometer.Servicebus.Subscribers
{
    public class ChangeStreamSubscriber : IChangeStreamSubscriber
    {
       
        private const string QUEUE_NAME = "changestream/notification";
        private readonly IServicebusSettings _settings;
        private readonly ILogger _logger;
        private readonly IQueueClient _queueClient;

        public ChangeStreamSubscriber(IServicebusSettings settings, ILoggerFactory loggerFactory)
        {
            _settings = settings;
            _logger = loggerFactory.CreateLogger<ChangeStreamSubscriber>();
            var builder = new ServiceBusConnectionStringBuilder()
            {
                Endpoint = _settings.Endpoint,
                EntityPath = _settings.EntityPath,
                SasKey = _settings.SasKey,
                SasKeyName = _settings.SasKeyName
            };
            _queueClient = new QueueClient(builder, ReceiveMode.PeekLock);

        }

        public void RegisterOnMessageHandlerAndReceiveMessages()
        {
            try
            {
               

                var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
                {
                    MaxConcurrentCalls = 1,
                    AutoComplete = false
                };
                _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var myPayload = JsonConvert.DeserializeObject<ChangeStream>(Encoding.UTF8.GetString(message.Body));
            
            //TODO:1. Quer the database to figure out centerId associated with users appId  & Get the list of onesignal appId (based on dated)
            //TODO:2. Notify using One Signal
            //TODO: 3. Delete centerIds from users from notitication

            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }


        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            _logger.LogError(exceptionReceivedEventArgs.Exception, "Message handler encountered an exception");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

            _logger.LogDebug($"- Endpoint: {context.Endpoint}");
            _logger.LogDebug($"- Entity Path: {context.EntityPath}");
            _logger.LogDebug($"- Executing Action: {context.Action}");

            return Task.CompletedTask;
        }

        public async Task CloseQueueAsync()
        {
           
          await _queueClient.CloseAsync();
        }
    }
}
