using System;
using System.Collections.Generic;
using System.Text;

namespace Vaxometer.Servicebus.Settings
{
    public interface IServicebusSettings
    {
        string ConnecionString { get; set; }
        string PrimaryKey { get; set; }
    }
}
