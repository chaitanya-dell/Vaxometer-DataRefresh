using System.Collections.Generic;
using System.Threading.Tasks;
using Vaxometer_DataRefresh.Models;
using Vaxometer_DataRefresh.Models.DataModels;
using Centers = Vaxometer_DataRefresh.Models.Centers;

namespace Vaxometer_DataRefresh.Repository
{
    public interface IDataRepository
    {
        Task<bool> Save(CentersData centersData, RefreshAuditTrail districtCode);
        public Task<IEnumerable<RefreshAuditTrail>> GetAuditTrails();


    }
}
