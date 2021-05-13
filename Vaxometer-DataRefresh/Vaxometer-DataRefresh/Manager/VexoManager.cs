using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Vaxometer_DataRefresh.Models;
using Vaxometer_DataRefresh.Repository;

namespace Vaxometer_DataRefresh.Manager
{
    public class VexoManager : IVexoManager
    {
        private readonly ICowinRepository _cowinRepository;
        private readonly IDataRepository _dataRepository;
        private readonly ILogger _logger;

        public VexoManager(ICowinRepository cowinRepository, IDataRepository dataRepository,
            ILoggerFactory loggerFactory)
        {
            _cowinRepository = cowinRepository;
            _dataRepository = dataRepository;
            _logger = loggerFactory.CreateLogger<VexoManager>();
        }


        public async Task<bool> RefershData(int districtCode)
        {
            try
            {
                var centersData = await _cowinRepository.GetCentersByDistrict(districtCode);

                //var centersData = await _cowinRepository.GetCentersForDistrict_294_265();
                //#if DEBUG
                //            return true;
                //#endif
                // return await _dataRepository.Save(centersData, districtCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return false;
        }

        public async Task<bool> RefreshAllDistricts()
        {
            try
            {
                var districtsList = await _dataRepository.GetAuditTrails();

                foreach (var district in districtsList)
                {
                    TimeSpan span = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now).Subtract(district.modified_at);

                    if ((district.status == RefreshDataStatus.Updated && span.TotalMinutes >= 30) ||
                        (district.status == RefreshDataStatus.New))
                    {
                        var centersData = await _cowinRepository.GetCentersByDistrict(district.district_id);
                        await _dataRepository.Save(centersData, district);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return false;
        }

    }
}
