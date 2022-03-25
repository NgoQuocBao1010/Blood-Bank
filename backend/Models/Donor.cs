using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class BloodDonor
    {
        public string name { get; set; }
        public string type { get; set; }
    }
    public class Donor
    {
        public string _id { get; set; }
        
        public string name { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public BloodDonor blood { get; set; }
        [BsonIgnore]
        public DonorTransaction transaction { get; set; }
    }
}