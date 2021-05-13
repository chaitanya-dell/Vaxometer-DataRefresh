using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Vaxometer_DataRefresh.Models.DataModels
{
    //[BsonCollection("centers")]
    //public class Centers : ICenter
    //{
    //    [BsonId]
    //    [BsonRepresentation(BsonType.ObjectId)]
    //    public ObjectId Id { get; set; }

    //    [BsonElement("center_id")]
    //    public int center_id { get; set; }

    //    [BsonElement("name")]
    //    public string name { get; set; }

    //    [BsonElement("state_name")]
    //    public string state_name { get; set; }

    //    [BsonElement("district_name")]
    //    public string district_name { get; set; }

    //    [BsonElement("block_name")]
    //    public string block_name { get; set; }

    //    [BsonElement("pincode")]
    //    public int pincode { get; set; }
    //    // public int lat { get; set; }
    //    //public int long { get; set; }

    //    [BsonElement("from")]
    //    public string from { get; set; }

    //    [BsonElement("to")]
    //    public string to { get; set; }

    //    [BsonElement("fee_type")]
    //    public string fee_type { get; set; }

    //    [BsonElement("fee_type")]
    //    public IList<Sessions> sessions { get; set; }

    //    [BsonElement("fee_type")]
    //    public IList<Vaccine_fees> vaccine_fees { get; set; }
    //}

    [BsonCollection("centers")]
    public class Centers : ICenter
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.String)]
        public ObjectId Id { get; set; }
        public int center_id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string state_name { get; set; }
        public string district_name { get; set; }
        public string block_name { get; set; }
        public int pincode { get; set; }
        public int lat { get; set; }
        [BsonElement("long")]
        public int @long { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string fee_type { get; set; }
        public List<Sessions> sessions { get; set; }
        public List<Vaccine_fees> vaccine_fees { get; set; }
        public DateTime CreatedModifiedAt { get; set; }

    }

    [BsonCollection("Sessions")]
    public class Sessions
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.String)]
        public ObjectId Id { get; set; }
        public string session_id { get; set; }
        public string date { get; set; }
        public int available_capacity { get; set; }
        public int available_capacity_dose1 { get; set; }
        public int available_capacity_dose2 { get; set; }
        public int min_age_limit { get; set; }
        public string vaccine { get; set; }
        public List<string> slots { get; set; }

    }

    [BsonCollection("Vaccine_fees")]
    public class Vaccine_fees
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string vaccine { get; set; }
        public string fee { get; set; }
    }

}
