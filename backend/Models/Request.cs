using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public abstract class RequestBlood
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public abstract class RequestHospital
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string Name { get; set; }
    }

    public class Request
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        
        public string Date { get; set; }
        public string RequestQuantity { get; set; }
        public RequestBlood RequestBlood { get; set; }
        public RequestHospital RequestHospital { get; set; }
    }
}