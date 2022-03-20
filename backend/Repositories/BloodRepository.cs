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

        public void AddDefaultData(List<Blood> listBlood)
        {
            _blood.InsertMany(listBlood);
        }

        public Task<Blood> Get(string _id)
        {
            var filter = Builders<Blood>.Filter.Eq(b => b._id, _id);
            var blood = _blood.Find(filter).FirstOrDefaultAsync();

            return blood;
        }
        
        public async Task<Blood> GetByName(string name)
        {
            var filter = Builders<Blood>.Filter.Eq(b => b.name, name);
            var blood = await _blood.Find(filter).FirstOrDefaultAsync();

            return blood;
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
                .Set(b => b.name, blood.name)
                .Set(b => b.type, blood.type)
                .Set(b => b.quantity, blood.quantity);
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