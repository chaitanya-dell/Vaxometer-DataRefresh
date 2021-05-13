using System.Collections.Generic;
using System.Threading.Tasks;
using Vaxometer_DataRefresh.Models;

namespace Vaxometer_DataRefresh.Repository
{
    public interface ICowinRepository
    {
        public Task<CentersData> GetCentersForDistrict_294_265();
        public Task<List<long>> NearestPinCode(int pinCode);
        public Task<CentersData> GetCentersByPinCode(int pinCode);
        public Task<CentersData> GetCentersByDistrict(int districtCode);
        public Task<bool> ValidatePinCode(int pincode);
    }
}
