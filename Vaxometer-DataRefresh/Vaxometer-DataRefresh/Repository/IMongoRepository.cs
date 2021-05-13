using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vaxometer_DataRefresh.Models.DataModels;

namespace Vaxometer_DataRefresh.Repository
{ 
    public interface IMongoRepository<T> where T : ICenter
    {
        Task CreateOrUpdate(List<T> collection, RefreshAuditTrail districtCode);
        Task<IEnumerable<RefreshAuditTrail>> GetAuditTrails();
    }
}
