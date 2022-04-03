namespace backend.Models
{
    public class Id
    {
        public string _id { get; set; }
        public string date { get; set; }
        public string type { get; set; }

        public Id(string id, string date, string type)
        {
            _id = id;
            this.date = date;
            this.type = type;
        }
    }
    
    public class RecentActivities
    {
        public string _id { get; set; }
        
        public string type { get; set; }
        public string detail { get; set; }
        public string date { get; set; }

        public RecentActivities(string id, string type, string detail, string date)
        {
            _id = id;
            this.type = type;
            this.detail = detail;
            this.date = date;
        }
    }
}