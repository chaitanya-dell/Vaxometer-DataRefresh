using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vaxometer_DataRefresh.Repository.DbSettings
{
    public interface IVexoDatabaseSettings
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
       
    }
}
