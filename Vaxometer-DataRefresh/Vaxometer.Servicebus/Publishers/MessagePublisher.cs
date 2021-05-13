using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Vaxometer.Servicebus.Publishers
{
    public class MessagePublisher : IMessagePublisher
    {
       
        public MessagePublisher()
        {
            
        }
       

        Task IMessagePublisher.PublisherAsync<T>(T request)
        {
            throw new NotImplementedException();
        }
    }
}
