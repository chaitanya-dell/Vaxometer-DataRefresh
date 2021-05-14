using System;
using System.Collections.Generic;
using System.Text;

namespace Vaxometer.Servicebus.Settings
{
    public class ServicebusSettings : IServicebusSettings
    {
        public string ConnecionString { get ; set ; }
        public string PrimaryKey { get; set; }
        public string Endpoint { get; set; }
        public string EntityPath { get; set; }
        public string SasKey { get; set; }
        public string SasKeyName { get; set; }
    }
}

