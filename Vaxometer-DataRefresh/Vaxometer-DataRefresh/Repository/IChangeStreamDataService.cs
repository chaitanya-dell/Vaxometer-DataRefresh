using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vaxometer_DataRefresh.Repository
{
    public interface IChangeStreamDataService
    {
        Task WatchSession();
    }
}
