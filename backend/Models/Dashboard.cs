using System.Collections.Generic;

namespace backend.Models
{

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

    public class Datasets
    {
        public string label { get; set; }
        public List<int> data { get; set; }

        public Datasets(string label, List<int> data)
        {
            this.label = label;
            this.data = data;
        }
    }
    
    public class Chart
    {
        public List<string> labels { get; set; }
        public List<Datasets>  datasets { get; set; }
    }
    
}