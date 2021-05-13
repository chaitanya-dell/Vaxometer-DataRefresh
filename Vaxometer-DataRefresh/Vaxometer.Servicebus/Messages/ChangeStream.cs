using System;
using System.Collections.Generic;
using System.Text;

namespace Vaxometer.Servicebus.Messages
{
    public class ChangeStream
    {
        public string  CenterId { get; set; }
        public string DistrictName { get; set; }
        public string PinCode { get; set; }
        public string AvailableSession { get; set; }
    }
}

