using System.Collections.Generic;
using System.Threading.Tasks;
using Vaxometer_DataRefresh.Models;

namespace Vaxometer_DataRefresh.Manager
{
    public interface IVexoManager
    {
        public Task<bool> RefershData(int districtCode);

        public Task<bool> RefreshAllDistricts();

    }
}
