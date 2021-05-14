using System;
using System.Collections.Generic;
using System.Text;

namespace Vaxometer.Servicebus.Messages
{
    [QueueName("changestream/notification")]
    public class ChangeStream
    {
        public int CenterId { get; set; }
        public string DistrictName { get; set; }
        public int PinCode { get; set; }
        public string AvailableSession { get; set; }
    }
}

