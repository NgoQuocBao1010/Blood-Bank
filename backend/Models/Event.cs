using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class Location
    {
        public string city { get; set; }
        public string address { get; set; }
    }
    public class Event
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string name { get; set; }
        public Location location { get; set; }
        public string startDate { get; set; }
        public int duration { get; set; }
        public string detail { get; set; }
        public int participants { get; set; }
    }
}