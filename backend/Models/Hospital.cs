using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class Hospital
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        
        public string hospital_name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
    }
}