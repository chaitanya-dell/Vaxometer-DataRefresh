using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Vaxometer_DataRefresh
{
    public interface IDistrict
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        ObjectId Id { get; set; }

        [BsonElement("district_id")]
        int district_id { get; set; }

        [BsonElement("district_name")]
        string district_name { get; set; }
    }
}
