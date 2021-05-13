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
