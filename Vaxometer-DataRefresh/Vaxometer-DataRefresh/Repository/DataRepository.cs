using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Vaxometer_DataRefresh.Manager;
using Vaxometer_DataRefresh.Models;
using Vaxometer_DataRefresh.Models.DataModels;
using Vaxometer_DataRefresh.Repository.DbSettings;

namespace Vaxometer_DataRefresh.Repository
{
    public class DataRepository : IDataRepository
    {
        private IMongoRepository<Models.DataModels.Centers> _mongoCenters;
        private readonly IVexoDatabaseSettings _settings;
        private readonly ILogger<DataRepository> _logger;
        private readonly IMapper _mapper;
        private readonly IVexoDataService _vexoDataService;


        public DataRepository(
            IVexoDatabaseSettings settings, ILogger<DataRepository> logger,
            IMongoRepository<Models.DataModels.Centers> mongoRepository, IMapper mapper,
            IVexoDataService vexoDataService)
        {
            _settings = settings;
            _logger = logger;
            _mongoCenters = mongoRepository;
            _mapper = mapper;
            _vexoDataService = vexoDataService;
        }

        public async Task<bool> Save(CentersData request, RefreshAuditTrail districtCode)
        {
            try
            {
                _mongoCenters = new MongoRepository<Models.DataModels.Centers>(_settings);
                var centersCollection = new List<Models.DataModels.Centers>();

                foreach (var center in request.Centers)
                    centersCollection.Add(_mapper.Map<Models.DataModels.Centers>(center));
                await _mongoCenters.CreateOrUpdate(centersCollection, districtCode);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogTrace(ex.Message, ex);

            }

            return false;
        }



        public async Task<IEnumerable<RefreshAuditTrail>> GetAuditTrails()
        {
            try
            {
                _mongoCenters = new MongoRepository<Models.DataModels.Centers>(_settings);
                var dataModel = await _mongoCenters.GetAuditTrails();
                return dataModel;
            }
            catch (Exception ex)
            {
                _logger.LogTrace(ex.Message, ex);
                throw ex;
            }
        }

    }
}
