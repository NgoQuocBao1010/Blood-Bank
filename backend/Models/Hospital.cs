using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class Hospital
    {
        public Hospital(string hospital_name, string address, string phone)
        {
            this.hospital_name = hospital_name;
            this.address = address;
            this.phone = phone;
        }
        
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        
        public string hospital_name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        
    }
}