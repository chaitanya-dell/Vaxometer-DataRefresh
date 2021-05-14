using Azure.Messaging.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vaxometer.Servicebus.Settings;

namespace Vaxometer.Servicebus.Publishers
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IServicebusSettings _settings;
        private readonly ILogger _logger;

        public MessagePublisher(IServicebusSettings settings, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MessagePublisher>();
            _settings = settings;
            
        }

       public  async Task  PublisherAsync<T>(T requestMessage)
        {
            try
            {
                var queuename = GetQueueName(typeof(T));

                var builder = new ServiceBusConnectionStringBuilder()
                {
                    Endpoint = _settings.Endpoint,
                    EntityPath = _settings.EntityPath,
                     SasKey = _settings.SasKey,
                       SasKeyName = _settings.SasKeyName
                };
                IQueueClient queueClient = new QueueClient(builder, ReceiveMode.PeekLock);
                var message = new Message
                {
                    MessageId = Guid.NewGuid().ToString(),
                    Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestMessage))
                };
                message.UserProperties.Add("MessageType", typeof(T).Name);
                await queueClient.SendAsync(message);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        private protected string GetQueueName(Type documentType)
        {
          
            var queuename = ((QueueNameAttribute)documentType.GetCustomAttributes(
                    typeof(QueueNameAttribute), true).FirstOrDefault())?.QueueName;
            return queuename;
        }
    }
}
