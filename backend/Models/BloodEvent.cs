using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class BloodEvent
    {
        // public ObjectId _id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string event_name { get; set; }
        public string location { get; set; }
        public string date { get; set; }
    }
}