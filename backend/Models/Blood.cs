using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class Blood
    {
        public Blood(string name, string type, int quantity)
        {
            this.name = name;
            this.type = type;
            this.quantity = quantity;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        
        public string name { get; set; }
        public string type { get; set; }
        public int quantity { get; set; }
    }
}