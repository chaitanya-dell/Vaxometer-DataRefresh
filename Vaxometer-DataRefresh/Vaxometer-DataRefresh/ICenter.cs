using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Vaxometer_DataRefresh.Models.DataModels;

namespace Vaxometer_DataRefresh
{
    public interface ICenter
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        ObjectId Id { get; set; }

        [BsonElement("center_id")]
        int center_id { get; set; }

        [BsonElement("name")]
        string name { get; set; }

        [BsonElement("state_name")]
        string state_name { get; set; }

        [BsonElement("district_name")]
        string district_name { get; set; }

        [BsonElement("block_name")]
        string block_name { get; set; }

        [BsonElement("pincode")]
        int pincode { get; set; }

        [BsonElement("lat")]
        int @lat { get; set; }

        [BsonElement("long")]
        int @long { get; set; }

        [BsonElement("from")]
        string from { get; set; }

        [BsonElement("to")]
        string to { get; set; }

        [BsonElement("fee_type")]
        string fee_type { get; set; }

        [BsonElement("sessions")]
        List<Sessions> sessions { get; set; }

        [BsonElement("vaccine_fees")]
        List<Vaccine_fees> vaccine_fees { get; set; }
        DateTime CreatedModifiedAt { get; set; }
        string address { get; set; }
    }




}
