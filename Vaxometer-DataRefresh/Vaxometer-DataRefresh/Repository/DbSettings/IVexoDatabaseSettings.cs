using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vaxometer_DataRefresh.Repository.DbSettings
{
    public class IVexoDatabaseSettings
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
       
    }
}
