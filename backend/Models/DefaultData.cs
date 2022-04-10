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

        public static async Task<DefaultData> ReadJson(string filePath)
        {
            var jsonText = await File.ReadAllTextAsync(filePath);
            var json = JsonConvert.DeserializeObject<DefaultData>(jsonText);
            return json;
        }
    }
}