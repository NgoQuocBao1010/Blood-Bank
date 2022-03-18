using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class Donor
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        
        public string fullname { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string blood_type { get; set; }
        public List<DonorTransaction> listTransaction { get; set; }
    }
}