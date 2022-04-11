using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace backend.Models
{
    public class DefaultData
    {
        public List<Hospital> Hospitals { get; set; }
        public List<User> Users { get; set; }
        public List<Event> Events { get; set; }
        public List<Blood> Blood { get; set; }
        public List<ListParticipants> Donors { get; set; }
        public List<EventSubmission> EventSubmissions { get; set; }
        public List<Request> Requests { get; set; }
        public static async Task<DefaultData> ReadJson()
        {
            var jsonText = await File.ReadAllTextAsync("default_data.json");
            var json = JsonConvert.DeserializeObject<DefaultData>(jsonText);
            return json;
        }
    }
}