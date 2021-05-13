using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Vaxometer.Servicebus.Publishers
{
    public interface IMessagePublisher
    {
        Task PublisherAsync<T>(T request);
    }

  
}
