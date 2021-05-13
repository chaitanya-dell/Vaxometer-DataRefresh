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
            IMongoRepository<Models.DataModels.Centers> mongoRepository, IMapper mapper, IVexoDataService vexoDataService)
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


        public async Task<IEnumerable<Models.Centers>> CentersByPinCode(int pincode)
        {
            _mongoCenters = new MongoRepository<Models.DataModels.Centers>(_settings);
            var dataModel = await _mongoCenters.CentersByPinCode(pincode);
            var collection = new List<Models.Centers>();
            foreach (var doc in dataModel)
                collection.Add(_mapper.Map<Models.Centers>(doc));
            return collection;
        }

        public async Task<List<Models.Centers>> CentersByPinCodeAndNearestPinCode(List<long> pincodeList)
        {
            _mongoCenters = new MongoRepository<Models.DataModels.Centers>(_settings);
            var dataModel = await _mongoCenters.CentersByPinCodeAndNearestPinCode(pincodeList);
            var collection = new List<Models.Centers>();
            foreach (var doc in dataModel)
                collection.Add(_mapper.Map<Models.Centers>(doc));
            return collection;
        }

        public async Task<IEnumerable<Models.Centers>> GetBangaloreCenterFor18yrs()
        {
            _mongoCenters = new MongoRepository<Models.DataModels.Centers>(_settings);
            var dataModel = await _mongoCenters.GetBangaloreCenterFor18yrs();
            var collection = new List<Models.Centers>();
            foreach (var doc in dataModel)
                collection.Add(_mapper.Map<Models.Centers>(doc));
            return collection;
        }


        public async Task<IEnumerable<Models.Centers>> GetBangaloreCenterFor45yrs()
        {
            _mongoCenters = new MongoRepository<Models.DataModels.Centers>(_settings);
            var dataModel = await _mongoCenters.GetBangaloreCenterFor45yrs();
            var collection = new List<Models.Centers>();
            foreach (var doc in dataModel)
                collection.Add(_mapper.Map<Models.Centers>(doc));
            return collection;
        }

        public bool CreateOne(Models.DataModels.Centers request)
        {
            try
            {
                _mongoCenters = new MongoRepository<Models.DataModels.Centers>(_settings);
                _mongoCenters.Create(request);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogTrace(ex.Message, ex);
                throw ex;
            }
        }

        public bool CreateMany(List<Models.DataModels.Centers> request)
        {
            try
            {
                _mongoCenters = new MongoRepository<Models.DataModels.Centers>(_settings);
                _mongoCenters.CreateMany(request);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogTrace(ex.Message, ex);
                throw ex;
            }
        }

        public async Task<IEnumerable<Models.Centers>> GetVaccineCenters(int age, decimal latitude, decimal longitude, long pincode, string vaccineType)
        {
            try
            {
                _mongoCenters = new MongoRepository<Models.DataModels.Centers>(_settings);
               var dataModel = await _mongoCenters.GetVaccineCenters(age,latitude,longitude,pincode,vaccineType);
               var collection = new List<Models.Centers>();
               foreach (var doc in dataModel)
                   collection.Add(_mapper.Map<Models.Centers>(doc));
               return collection;
            }
            catch (Exception ex)
            {
                _logger.LogTrace(ex.Message, ex);
                throw ex;
            }
        }
        Task<IEnumerable<Models.Centers>> IDataRepository.GetBangaloreCenterFor18yrsCovaxin()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Models.Centers>> IDataRepository.GetBangaloreCenterFor45yrsCovaxin()
        {
            throw new NotImplementedException();
        }



        public Task<List<Models.DataModels.Centers>> GetBangaloreCenterFor18yrsCovaxin()
        {
            throw new NotImplementedException();
        }

       

        public Task<List<Models.DataModels.Centers>> GetBangaloreCenterFor45yrsCovaxin()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Models.Centers>> GetBangaloreCenterFor18yrsCoviShield()
        {
            _mongoCenters = new MongoRepository<Models.DataModels.Centers>(_settings);
            var dataModel = await _mongoCenters.GetBangaloreCenterFor18yrsCoviShield();
            var collection = new List<Models.Centers>();
            foreach (var doc in dataModel)
                collection.Add(_mapper.Map<Models.Centers>(doc));
            return collection;
        }

        public async Task<IEnumerable<Models.Centers>> GetBangaloreCenterFor45yrsCovishield()
        {
            _mongoCenters = new MongoRepository<Models.DataModels.Centers>(_settings);
            var dataModel = await _mongoCenters.GetBangaloreCenterFor45yrsCovishield();
            var collection = new List<Models.Centers>();
            foreach (var doc in dataModel)
                collection.Add(_mapper.Map<Models.Centers>(doc));
            return collection;
        }

        public async Task<bool> SaveDistrictIdByPinCode(string districtName)
        {
            try
            {
                _mongoCenters = new MongoRepository<Models.DataModels.Centers>(_settings);
                await _mongoCenters.SaveDistrictIdByPinCode(districtName);
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

        public async Task CreateWatchOnSessions()
        {
            try
            {
                await _vexoDataService.WatchSession();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
        }
    }
}
