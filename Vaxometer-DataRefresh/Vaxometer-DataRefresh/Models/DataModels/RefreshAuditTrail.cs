using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Vaxometer_DataRefresh.Models.DataModels
{
    [BsonCollection("refreshAuditTrail")]
    public class RefreshAuditTrail : IRefreshAuditTrail
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId Id { get; set; }
        public int district_id { get; set; }
        public DateTime modified_at { get; set; }
        public RefreshDataStatus status { get; set; }
        public string district_name { get; set; }
    }
}