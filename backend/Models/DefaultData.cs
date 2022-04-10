using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace backend.Models
{
    public class DefaultData
    {
        public List<Hospital> Hospitals;
        public List<User> Users;
        
        public async Task<DefaultData> ReadJson(string filePath)
        {
            var jsonText = await File.ReadAllTextAsync(filePath);
            var json = JsonConvert.DeserializeObject<DefaultData>(jsonText);
            return json;
        }
    }
}