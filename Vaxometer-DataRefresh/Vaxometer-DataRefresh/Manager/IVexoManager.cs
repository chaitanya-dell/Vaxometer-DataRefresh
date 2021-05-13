using System.Collections.Generic;
using System.Threading.Tasks;
using Vaxometer_DataRefresh.Models;

namespace Vaxometer_DataRefresh.Manager
{
    public interface IVexoManager
    {
        public Task<bool> RefershData(int districtCode);

        public Task<bool> RefreshAllDistricts();

        public Task<IEnumerable<Centers>> GetCentersByPinCode(int pincode);
        public Task<IEnumerable<Centers>> GetCentersByPinCodeAndNearbyPincode(int pincode);

        public Task<IEnumerable<Centers>> GetBangaloreCenterFor18yrs();

        public Task<IEnumerable<Centers>> GetBangaloreCenterFor45yrs();

        public Task<IEnumerable<Centers>> GetBangaloreCenterFor18yrsCovaxin();

        public Task<IEnumerable<Centers>> GetBangaloreCenterFor45yrsCovaxin();

        public Task<IEnumerable<Centers>> GetBangaloreCenterFor18yrsCoviShield();

        public Task<IEnumerable<Centers>> GetBangaloreCenterFor45yrsCovishield();
        public Task<bool> ValidatePinCode(int pincode);
    }
}
