using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace backend.Repositories
{
    public class HospitalRepository : IHospitalRepository
    {
        private readonly IMongoCollection<Hospital> _hospital;

        public HospitalRepository(IMongoClient client)
        {
            var database = client.GetDatabase("BloodBank");
            var collection = database.GetCollection<Hospital>(nameof(Hospital));

            _hospital = collection;
            AddDefaultData();
        }

        public void AddDefaultData()
        {
            var hospital = Get();
            if (hospital.Result.Any()) return;
            // var listHospital = new List<Hospital>
            // {
            //     new("Hoan My", "Can Tho", "9876543210"),
            //     new("Da Khoa Trung Uong", "Can Tho", "0123456789"),
            //     new("121", "Can Tho", "0123456780"),
            // };
            // var defaultData = userRepository.ReadJson("default_data.json");
            var defaultData = new DefaultData();
            var data = defaultData.ReadJson("default_data.json");

            _hospital.InsertMany(data.Result.Hospitals);
        }


        public async Task<string> Create(Hospital hospital)
        {
            await _hospital.InsertOneAsync(hospital);
            return hospital._id;
        }

        public Task<Hospital> Get(string _id)
        {
            var filter = Builders<Hospital>.Filter.Eq(b => b._id, _id);
            var hospital = _hospital.Find(filter).FirstOrDefaultAsync();

            return hospital;
        }

        public Task<Hospital> GetFirstHospital()
        {
            var hospital = _hospital.Find(_ => true).FirstOrDefaultAsync();

            return hospital;
        }

        public async Task<IEnumerable<Hospital>> Get()
        {
            var hospital = await _hospital.Find(_ => true).ToListAsync();
            
            return hospital;
        }


        public async Task<long> Update(string _id, Hospital hospital)
        {
            var filter = Builders<Hospital>.Filter.Eq(b => b._id, _id);
            var update = Builders<Hospital>.Update
                .Set(h => h.Name, hospital.Name)
                .Set(h => h.Address, hospital.Address)
                .Set(h => h.Phone, hospital.Phone)
                .Set(h => h.RequestHistory, hospital.RequestHistory);

            var result = await _hospital.UpdateOneAsync(filter, update);

            return result.ModifiedCount;
        }

        public async Task<bool> Delete(string _id)
        {
            var filter = Builders<Hospital>.Filter.Eq(b => b._id, _id);
            var result = await _hospital.DeleteOneAsync(filter);

            return result.DeletedCount == 1;
        }
    }
}