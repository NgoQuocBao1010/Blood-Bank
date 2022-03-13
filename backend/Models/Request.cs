using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class Request
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        
        public string date { get; set; }
        public string volume { get; set; }
        public string blood_id { get; set; }
        public string hospital_id { get; set; }
    }
}