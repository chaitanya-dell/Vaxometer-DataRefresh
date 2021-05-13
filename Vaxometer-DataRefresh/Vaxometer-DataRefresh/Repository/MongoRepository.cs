using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Vaxometer_DataRefresh.Models;
using Vaxometer_DataRefresh.Models.DataModels;
using Vaxometer_DataRefresh.Repository.DbSettings;

namespace Vaxometer_DataRefresh.Repository
{
    public class MongoRepository<T> : IMongoRepository<T> where T : ICenter
    {
        private readonly IMongoCollection<T> _collection;
        private readonly IMongoCollection<District> _districtCollection;
        private readonly IMongoCollection<RefreshAuditTrail> _auditTrailCollection;
        private const string COVISHIELD = "COVISHIELD";
        public MongoRepository(IVexoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<T>(GetCollectionName(typeof(T)));
            _districtCollection = database.GetCollection<District>("districts");
            _auditTrailCollection = database.GetCollection<RefreshAuditTrail>("refreshAuditTrail");
        }

        private protected string GetCollectionName(Type documentType)
        {
            var collectionName = ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
            return collectionName;
        }

        public async Task CreateOrUpdate(List<T> collection, RefreshAuditTrail district)
        {
            await UpdateRefreshAuditTrail(district.district_id, RefreshDataStatus.InProgress, district.district_name);


            var bulkOps = new List<WriteModel<T>>();
            foreach (var record in collection)
            {
                var upsertOne = new UpdateOneModel<T>(
                    Builders<T>.Filter.Where(x => x.center_id == record.center_id),
                     Builders<T>.Update
                    .Set(p => p.block_name, record.block_name)
                    .Set(p => p.center_id, record.center_id)
                    .Set(p => p.district_name, record.district_name)
                    .Set(p => p.fee_type, record.fee_type)
                    .Set(p => p.name, record.name)
                    .Set(p => p.pincode, record.pincode)
                    .Set(p => p.state_name, record.state_name)
                    .Set(p => p.from, record.from)
                    .Set(p => p.to, record.to)
                    .Set(p => p.sessions, record.sessions)
                    .Set(p => p.vaccine_fees, record.vaccine_fees)
                    .Set(p => p.CreatedModifiedAt, TimeZoneInfo.ConvertTimeToUtc(DateTime.Now))
                    .Set(p => p.address, record.address)
                    .Set(p => p.lat, record.lat)
                    .Set(p => p.@long, record.@long))
                { IsUpsert = true };
                bulkOps.Add(upsertOne);
            }
            var resultWrites = await _collection.BulkWriteAsync(bulkOps);



            //foreach (var d in collection)
            //{
            //    await _collection.UpdateOneAsync<T>(x => x.center_id == d.center_id,
            //        Builders<T>.Update
            //        .Set(p => p.block_name, d.block_name)
            //        .Set(p => p.center_id, d.center_id)
            //        .Set(p => p.district_name, d.district_name)
            //        .Set(p => p.fee_type, d.fee_type)
            //        .Set(p => p.name, d.name)
            //        .Set(p => p.pincode, d.pincode)
            //        .Set(p => p.state_name, d.state_name)
            //        .Set(p => p.from, d.from)
            //        .Set(p => p.to, d.to)
            //        .Set(p => p.sessions, d.sessions)
            //        .Set(p => p.vaccine_fees, d.vaccine_fees)
            //        .Set(p => p.CreatedModifiedAt, TimeZoneInfo.ConvertTimeToUtc(DateTime.Now))
            //        .Set(p => p.address, d.address),
            //         new UpdateOptions { IsUpsert = true });

            //    foreach (var s in d.sessions)
            //    {
            //        //if (s.available_capacity > 0)
            //        //{


            //        var filter = Builders<T>.Filter.And(
            //            Builders<T>.Filter.Eq(x => x.center_id, d.center_id),
            //            Builders<T>.Filter.ElemMatch(x => x.sessions, x => x.session_id == s.session_id));

            //        var update = Builders<T>.Update
            //             .Set(p => p.sessions[-1].session_id, s.session_id)
            //             .Set(p => p.sessions[-1].available_capacity, s.available_capacity)
            //             .Set(p => p.sessions[-1].date, s.date)
            //             .Set(p => p.sessions[-1].min_age_limit, s.min_age_limit)

            //             .Set(p => p.sessions[-1].vaccine, s.vaccine);

            //        var session = _collection.Find(filter).SingleOrDefault();
            //        await _collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
            //        //}

            //    }

            //    foreach (var v in d.vaccine_fees)
            //    {
            //        var filter = Builders<T>.Filter;
            //        var secondFilter = filter.And(
            //            filter.Eq(x => x.center_id, d.center_id),
            //            filter.ElemMatch(x => x.vaccine_fees, c => c.vaccine == v.vaccine)
            //            );

            //        var update = Builders<T>.Update
            //             .Set(p => p.vaccine_fees[-1].vaccine, v.vaccine)
            //             .Set(p => p.vaccine_fees[-1].fee, v.fee);

            //        var vacc = _collection.Find(secondFilter).SingleOrDefault();
            //        await _collection.UpdateOneAsync(secondFilter, update, new UpdateOptions { IsUpsert = true });
            //    }
            //}

            await UpdateRefreshAuditTrail(district.district_id, RefreshDataStatus.Updated, district.district_name);

        }




        public void Create(T item)
        {
            _collection.InsertOne(item);
        }

        public void CreateMany(List<T> items)
        {
            _collection.InsertMany(items);
        }

        public async Task<IEnumerable<T>> CentersByPinCode(int pincode)
        {
            var result = await _collection.Find(x => x.pincode == pincode).ToListAsync();
            return result;
        }

        public async Task<List<T>> CentersByPinCodeAndNearestPinCode(List<long> pincodeList)
        {
            var filterDef = new FilterDefinitionBuilder<T>();
            var filter = filterDef.In(x => x.pincode, pincodeList);
            var result = await _collection.Find(filter).ToListAsync();
            return result;
        }


        public async Task<IEnumerable<T>> GetBangaloreCenterFor18yrs()
        {
            var builder = Builders<T>.Filter;
            var filter = builder.ElemMatch(x => x.sessions, y => y.min_age_limit == 18);
            return await _collection.Find(filter).ToListAsync();


            //Commenting this code as there will no center with 294, 265 and 276

            //var filter = Builders<T>.Filter.And(
            //           Builders<T>.Filter.Eq(x => x.center_id, 294) | Builders<T>.Filter.Eq(x => x.center_id, 265) | Builders<T>.Filter.Eq(x => x.center_id, 276),
            //           Builders<T>.Filter.ElemMatch(x => x.sessions, x => x.min_age_limit == 18));
            //var vacc = _collection.Find(filter).SingleOrDefault();
            //return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetBangaloreCenterFor45yrs()
        {
            var filter = Builders<T>.Filter.ElemMatch(x => x.sessions, x => x.min_age_limit == 45);

            //Commenting this line as there are more than one records and SingleOrDefault breaking application (More than one sequence.)
            //var vacc = _collection.Find(filter).SingleOrDefault();
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetVaccineCenters(int age, decimal latitude, decimal longitude, long pincode,
            string vaccineType)
        {
            var filter1 = Builders<T>.Filter.Empty;
            var filter2 = Builders<T>.Filter.Empty;
            var filter3 = Builders<T>.Filter.Empty;
            var filter4 = Builders<T>.Filter.Empty;
            if (age > 0)
                filter1 = Builders<T>.Filter.ElemMatch(x => x.sessions, x => (x.min_age_limit == age));
            if (latitude != 0 && longitude != 0)
                filter2 = Builders<T>.Filter.Where(x => (x.@long == longitude && x.lat == latitude));
            if (pincode != 0)
                filter3 = Builders<T>.Filter.Where(x => x.pincode == pincode);
            if (!string.IsNullOrEmpty(vaccineType))
                filter4 = Builders<T>.Filter.ElemMatch(x => x.sessions, x => x.vaccine == vaccineType);


            return await _collection.Find(filter1 & filter2 & filter3 & filter4).ToListAsync();
        }


        public virtual IEnumerable<T> FilterBy(Expression<Func<T, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }



        public virtual Task<T> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<T>.Filter.Eq(doc => doc.Id, objectId);
                return _collection.Find(filter).SingleOrDefaultAsync();
            });
        }

        public virtual T FindOne(Expression<Func<T, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public virtual Task<T> FindOneAsync(Expression<Func<T, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
        }

        public void InsertMany(ICollection<T> documents)
        {
            throw new NotImplementedException();
        }

        public Task InsertManyAsync(ICollection<T> documents)
        {
            throw new NotImplementedException();
        }

        public void InsertOne(T document)
        {
            throw new NotImplementedException();
        }

        public Task InsertOneAsync(T document)
        {
            throw new NotImplementedException();
        }

        public void ReplaceOne(T document)
        {
            throw new NotImplementedException();
        }

        public Task ReplaceOneAsync(T document)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> AsQueryable()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetBangaloreCenterFor18yrsCoviShield()
        {
            var builder = Builders<T>.Filter;
            var filter = builder.ElemMatch(x => x.sessions, y => y.min_age_limit == 18 && y.vaccine.ToLower() == COVISHIELD.ToLower());
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetBangaloreCenterFor45yrsCovishield()
        {
            var builder = Builders<T>.Filter;
            var filter = builder.ElemMatch(x => x.sessions, y => y.min_age_limit == 45 && y.vaccine.ToLower() == COVISHIELD.ToLower());
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task SaveDistrictIdByPinCode(string districtName)
        {
            var result = await _districtCollection.Find(x => x.district_name == districtName).FirstOrDefaultAsync();
            if (result != null)
            {
                await UpdateRefreshAuditTrail(result.district_id, RefreshDataStatus.New, districtName);
            }
        }

        private async Task UpdateRefreshAuditTrail(int districtId, RefreshDataStatus status, string districtName)
        {
            var filter = Builders<RefreshAuditTrail>.Filter.Eq(x => x.district_id, districtId);

            var update = Builders<RefreshAuditTrail>.Update
                 .Set(p => p.district_id, districtId)
                 .Set(p => p.modified_at, TimeZoneInfo.ConvertTimeToUtc(DateTime.Now))
                 .Set(p => p.status, status)
                 .Set(p => p.district_name, districtName);

            await _auditTrailCollection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        public async Task<IEnumerable<RefreshAuditTrail>> GetAuditTrails()
        {
            var auditTrailDistricts = await _auditTrailCollection.Find(Builders<RefreshAuditTrail>.Filter.Empty).ToListAsync();

            return auditTrailDistricts;
        }
    }
}
