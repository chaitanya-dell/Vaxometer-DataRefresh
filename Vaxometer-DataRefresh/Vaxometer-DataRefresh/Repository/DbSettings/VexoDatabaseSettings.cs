using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vaxometer_DataRefresh.Repository.DbSettings
{
    public class VexoDatabaseSettings : IVexoDatabaseSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        
    }
}
