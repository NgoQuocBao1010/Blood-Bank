using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class Participant
    {
        public string _id { get; set; }
        public string eventId { get; set; }
        public string rejectReason { get; set; }
    }
    
    public class EventDonated
    {
        public string _id { get; set; }
        public string name { get; set; }
    }
    
    
    public class DonorTransaction
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public BloodDonor blood { get; set; }
        public EventDonated eventDonated { get; set; }
        public string dateDonated { get; set; }
        public int amount { get; set; }
        public int status { get; set; }
        public string updateStatusAt { get; set; }
        public string rejectReason { get; set; }
        public string donorId { get; set; }
    }
}