using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class Location
    {
        public Location(string city, string address)
        {
            this.city = city;
            this.address = address;
        }

        public string city { get; set; }
        public string address { get; set; }
    }

    public class Event
    {
        public Event(string name, Location location, string startDate, int duration, string detail)
        {
            this.name = name;
            this.location = location;
            this.startDate = startDate;
            this.duration = duration;
            this.detail = detail;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public string name { get; set; }
        public Location location { get; set; }
        public string startDate { get; set; }
        public int duration { get; set; }
        public string detail { get; set; }
        [BsonIgnore]
        public int participants { get; set; }
    }
}