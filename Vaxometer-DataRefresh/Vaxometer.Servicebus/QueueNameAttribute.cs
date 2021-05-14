using System;
using System.Collections.Generic;
using System.Text;

namespace Vaxometer.Servicebus
{
   
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class QueueNameAttribute : Attribute
    {
        public string QueueName { get; }

        public QueueNameAttribute(string queueName)
        {
            QueueName = queueName;
        }
    }
}
