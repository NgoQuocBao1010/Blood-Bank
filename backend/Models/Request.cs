using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class RequestBlood
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class RequestHospital
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
        public int Quantity { get; set; }
        public RequestBlood Blood { get; set; }
        public RequestHospital Hospital { get; set; }
    }
}