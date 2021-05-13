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
        void Create(T item);
        void CreateMany(List<T> items);

        Task SaveDistrictIdByPinCode(string districtName); 
        Task<IEnumerable<T>> CentersByPinCode(int pincode);

        Task<List<T>> CentersByPinCodeAndNearestPinCode(List<long> pincodeList);
        Task<IEnumerable<T>> GetBangaloreCenterFor18yrs();
        Task<IEnumerable<T>> GetBangaloreCenterFor45yrs();

        Task<IEnumerable<T>> GetVaccineCenters(int age, decimal latitude, decimal longitude, long pincode,
            string vaccineType);
        Task<IEnumerable<T>> GetBangaloreCenterFor18yrsCoviShield();
        Task<IEnumerable<T>> GetBangaloreCenterFor45yrsCovishield();

        IQueryable<T> AsQueryable();

        IEnumerable<T> FilterBy(
            Expression<Func<T, bool>> filterExpression);

        IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, TProjected>> projectionExpression);

        T FindOne(Expression<Func<T, bool>> filterExpression);

        Task<T> FindOneAsync(Expression<Func<T, bool>> filterExpression);

     
        Task<T> FindByIdAsync(string id);

        void InsertOne(T document);

        Task InsertOneAsync(T document);

        void InsertMany(ICollection<T> documents);

        Task InsertManyAsync(ICollection<T> documents);

        void ReplaceOne(T document);

        Task ReplaceOneAsync(T document);
        Task<IEnumerable<RefreshAuditTrail>> GetAuditTrails();
    }
}
