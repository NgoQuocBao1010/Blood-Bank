using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Repositories
{
    public class BloodRepository : IBloodRepository
    {
        private static IMongoCollection<Blood> _blood;

        public BloodRepository(IMongoClient client)
        {
            var database = client.GetDatabase("BloodBank");
            var collection = database.GetCollection<Blood>(nameof(Blood));

            _blood = collection;
            AddDefaultData();
        }


        public async Task<string> Create(Blood blood)
        {
            await _blood.InsertOneAsync(blood);
            return blood._id;
        }
        
        public void AddDefaultData()
        {
            var blood = Get();
            if (blood.Result.Any()) return;
            var data = DefaultData.ReadJson();
            _blood.InsertMany(data.Result.Blood);
        }

        public Task<Blood> Get(string _id)
        {
            var filter = Builders<Blood>.Filter.Eq(b => b._id, _id);
            var blood = _blood.Find(filter).FirstOrDefaultAsync();

            return blood;
        }

        public async Task<Blood> GetByNameAndType(string name, string type)
        {
            var filter = Builders<Blood>.Filter.Eq(b => b.name, name)
                         & Builders<Blood>.Filter.Eq(b => b.type, type);
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

        public async Task<bool> UpdateQuantity(string name, string type, int amount)
        {
            var blood = await GetByNameAndType(name, type);
            // get current quantity of Blood
            var quantity = blood.quantity;
            
            // filter to update a quantity property
            var filter = Builders<Blood>.Filter.Eq(b => b.name, name)
                         & Builders<Blood>.Filter.Eq(b => b.type, type);
            var update = Builders<Blood>.Update
                .Set(b => b.quantity, quantity + amount);
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