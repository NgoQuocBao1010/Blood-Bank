using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Repositories
{
    public class BloodRepository : IBloodRepository
    {
        private readonly IMongoCollection<Blood> _blood;

        public BloodRepository(IMongoClient client)
        {
            var database = client.GetDatabase("BloodBank");
            var collection = database.GetCollection<Blood>(nameof(Blood));

            _blood = collection;
        }


        public async Task<string> Create(Blood blood)
        {
            await _blood.InsertOneAsync(blood);
            return blood._id;
        }

        public Task<Blood> Get(string _id)
        {
            var filter = Builders<Blood>.Filter.Eq(b => b._id, _id);
            var blood = _blood.Find(filter).FirstOrDefaultAsync();

            return blood;
        }
        
        public async Task<bool> GetByType(string blood_type)
        {
            var filter = Builders<Blood>.Filter.Eq(b => b.blood_type, blood_type);
            var blood = await _blood.Find(filter).ToListAsync();

            return blood.Count() > 0;
        }
        

        public async Task<IEnumerable<Blood>> Get()
        {
            var blood = await _blood.Find(_ => true).ToListAsync();

            return blood;
        }

        public async Task<bool> Update(string _id, Blood blood)
        {
            var filter = Builders<Blood>.Filter.Eq(b => b._id, _id);
            var update = Builders<Blood>.Update
                .Set(b => b.blood_type, blood.blood_type)
                .Set(b => b.quantity, blood.quantity)
                .Set(b => b.description, blood.description);

            var result = await _blood.UpdateOneAsync(filter, update);
            
            return result.ModifiedCount == 1;
        }

        public async Task<bool> Delete(string _id)
        {
            var filter = Builders<Blood>.Filter.Eq(b => b._id, _id);
            var result = await _blood.DeleteOneAsync(filter);

            return result.DeletedCount == 1;
        }
    }
}