using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Vaxometer_DataRefresh.Models;

namespace Vaxometer_DataRefresh
{
    public interface IRefreshAuditTrail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        ObjectId Id { get; set; }

        [BsonElement("district_id")]
        int district_id { get; set; }

        [BsonElement("modified_at")]
        DateTime modified_at { get; set; }

        [BsonElement("status")]
        RefreshDataStatus status { get; set; }

    }
}
