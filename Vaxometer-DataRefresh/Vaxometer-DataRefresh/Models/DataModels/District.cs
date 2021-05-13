using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Vaxometer_DataRefresh.Models.DataModels
{
    [BsonCollection("districts")]
    public class District : IDistrict
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public ObjectId Id { get; set; }
        public int district_id { get; set; }
        public string district_name { get; set; }
    }
}