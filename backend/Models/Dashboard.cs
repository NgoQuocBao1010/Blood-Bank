namespace backend.Models
{
    public class Id
    {
        public string _id { get; set; }
        public string date { get; set; }
        public string type { get; set; }
        public int amount { get; set; }

        public Id(string id, string date, string type, int amount)
        {
            _id = id;
            this.date = date;
            this.type = type;
            this.amount = amount;
        }
    }
    
    public class RecentActivities
    {
        public string _id { get; set; }
        
        public string type { get; set; }
        public string detail { get; set; }
        public string date { get; set; }
        public int amount { get; set; }

        public RecentActivities(string id, string type, string detail, string date, int amount)
        {
            _id = id;
            this.type = type;
            this.detail = detail;
            this.date = date;
            this.amount = amount;
        }
    }
}