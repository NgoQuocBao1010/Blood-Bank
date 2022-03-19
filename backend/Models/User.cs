using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class User
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string email { get; set; }
        public string password { get; set; }
        public bool isAdmin { get; set; }
        
        [BsonRepresentation(BsonType.ObjectId)]
        public string hospital_id { get; set; }
    }
}