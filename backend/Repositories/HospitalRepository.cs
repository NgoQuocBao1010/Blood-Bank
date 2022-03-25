using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
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

        public async Task<IEnumerable<Hospital>> Get()
        {
            var hospital = await _hospital.Find(_ => true).ToListAsync();

            return hospital;
        }

        public async Task<bool> Update(string _id, Hospital hospital)
        {
            var filter = Builders<Hospital>.Filter.Eq(b => b._id, _id);
            var update = Builders<Hospital>.Update
                .Set(h => h.hospital_name, hospital.hospital_name)
                .Set(h => h.address, hospital.address)
                .Set(h => h.phone, hospital.phone);

            var result = await _hospital.UpdateOneAsync(filter, update);
            
            return result.ModifiedCount == 1;
        }

        public async Task<bool> Delete(string _id)
        {
            var filter = Builders<Hospital>.Filter.Eq(b => b._id, _id);
            var result = await _hospital.DeleteOneAsync(filter);

            return result.DeletedCount == 1;
        }

        public void AddDefaultData(List<Hospital> listHospital)
        {
            _hospital.InsertMany(listHospital);
        }
    }
}