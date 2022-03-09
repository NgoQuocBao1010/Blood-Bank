using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class Blood
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        
        public string blood_type { get; set; }
        public int quantity { get; set; }
        public string description { get; set; }
    }
}