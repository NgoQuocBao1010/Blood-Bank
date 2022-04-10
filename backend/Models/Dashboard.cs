namespace backend.Models
{
    public class Id
    {
        public string _id { get; set; }
        public string date { get; set; }
        public string type { get; set; }
        public string transactionId { get; set; }
        public int amount { get; set; }
        
        public Id(string id, string date, string type,string transactionId, int amount)
        {
            _id = id;
            this.date = date;
            this.type = type;
            this.transactionId = transactionId;
            this.amount = amount;
        }
    }
    
    public class RecentActivities
    {
        public string _id { get; set; }
        
        public string type { get; set; }
        public string detail { get; set; }
        public string date { get; set; }
        public string transactionId { get; set; }
        public int amount { get; set; }


        public RecentActivities(string id, string type, string detail, string date, string transactionId, int amount)
        {
            _id = id;
            this.type = type;
            this.detail = detail;
            this.date = date;
            this.transactionId = transactionId;
            this.amount = amount;
        }
    }

    public class BloodReceive
    {
        public float total { get; set; }
        public float lastQuarter { get; set; }

        public BloodReceive(float total, float lastQuarter)
        {
            this.total = total;
            this.lastQuarter = lastQuarter;
        }
    }
    
    public class BloodDonated
    {
        public float total { get; set; }
        public float lastQuarter { get; set; }

        public BloodDonated(float total, float lastQuarter)
        {
            this.total = total;
            this.lastQuarter = lastQuarter;
        }
    }

    public class Donators
    {
        public int total { get; set; }
        public int lastQuarter { get; set; }

        public Donators(int total, int lastQuarter)
        {
            this.total = total;
            this.lastQuarter = lastQuarter;
        }
    }
    
    public class Events
    {
        public int total { get; set; }
        public int lastQuarter { get; set; }

        public Events(int total, int lastQuarter)
        {
            this.total = total;
            this.lastQuarter = lastQuarter;
        }
    }

    public class DashboardInfo
    {
        public BloodReceive bloodReceive { get; set; }
        public BloodDonated bloodDonated { get; set; }
        public Donators donators { get; set; }
        public Events events { get; set; }

        public DashboardInfo(BloodReceive bloodReceive, BloodDonated bloodDonated, Donators donators, Events events)
        {
            this.bloodReceive = bloodReceive;
            this.bloodDonated = bloodDonated;
            this.donators = donators;
            this.events = events;
        }
    }
    
    
}