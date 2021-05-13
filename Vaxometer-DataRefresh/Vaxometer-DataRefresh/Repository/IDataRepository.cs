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
        bool CreateOne(Models.DataModels.Centers request);
        bool CreateMany(List<Models.DataModels.Centers> request);
        public Task<IEnumerable<RefreshAuditTrail>> GetAuditTrails();
        public Task<IEnumerable<Centers>> CentersByPinCode(int pincode);
        public Task<List<Centers>> CentersByPinCodeAndNearestPinCode(List<long> pincodeList);
        Task<bool> SaveDistrictIdByPinCode(string districtName);
        public Task<IEnumerable<Centers>> GetBangaloreCenterFor18yrs();
        public Task<IEnumerable<Centers>> GetBangaloreCenterFor45yrs();
        Task<IEnumerable<Centers>> GetBangaloreCenterFor18yrsCovaxin();
        Task<IEnumerable<Centers>> GetBangaloreCenterFor45yrsCovaxin();


        Task<IEnumerable<Centers>> GetVaccineCenters(int age, decimal latitude, decimal longitude, long pincode,
            string vaccineType);

        public Task<IEnumerable<Centers>> GetBangaloreCenterFor18yrsCoviShield();
        public Task<IEnumerable<Centers>> GetBangaloreCenterFor45yrsCovishield();

        public Task CreateWatchOnSessions();

    }
}
