namespace backend.Models
{
    public class Id
    {
        public string _id { get; set; }
        public int date { get; set; }
    }
    
    public class RecentActivities
    {
        public string _id { get; set; }
        
        public string type { get; set; }
        public string detail { get; set; }
        public int date { get; set; }
    }
}