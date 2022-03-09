using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class DonorTransaction
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        
        public string donate_date { get; set; }
        public string volume { get; set; }
        public string donor_id { get; set; }
    }
}