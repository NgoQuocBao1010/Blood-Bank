using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class Id
    {
        public string _id { get; set; }
        public string date { get; set; }
        public string type { get; set; }
        public string transactionId { get; set; }
        public int amount { get; set; }
        
        public Id(string id, string date, string type, string transactionId, int amount)
        {
            _id = id;
            this.date = date;
            this.type = type;
            this.transactionId = transactionId;
            this.amount = amount;
        }
    }
    
    public class RecentActivity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string trigger { get; set; }
        public string triggerId { get; set; }
        public string detailId { get; set; }
        public string type { get; set; }
        public string detail { get; set; }
        public string date { get; set; }

        public int amount { get; set; }


        public RecentActivity(string trigger, string triggerId, string detailId, string type, string detail, string date, int amount)
        {
            this.trigger = trigger;
            this.triggerId = triggerId;
            this.detailId = detailId;
            this.type = type;
            this.detail = detail;
            this.date = date;
            this.amount = amount;
        }
    }
}