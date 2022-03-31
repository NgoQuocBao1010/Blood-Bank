using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class RequestBlood
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class Request
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string Date { get; set; }
        public int Quantity { get; set; }
        public RequestBlood Blood { get; set; }
        public string HospitalId { get; set; }
        public string HospitalName { get; set; }
        public int Status { get; set; }
        public string RejectReason { get; set; }
    }
}