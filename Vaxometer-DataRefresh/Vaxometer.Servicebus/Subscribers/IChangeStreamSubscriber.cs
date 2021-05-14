using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Vaxometer.Servicebus.Subscribers
{
    public interface IChangeStreamSubscriber
    {
        void RegisterOnMessageHandlerAndReceiveMessages();
        Task CloseQueueAsync();
    }
}
