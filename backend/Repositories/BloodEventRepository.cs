using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Repositories
{
    public class BloodEventRepository : IBloodEventRepository
    {
        private readonly IMongoCollection<BloodEvent> _bloodEvent;

        public BloodEventRepository(IMongoClient client)
        {
            var database = client.GetDatabase("BloodBank");
            var collection = database.GetCollection<BloodEvent>(nameof(BloodEvent));

            _bloodEvent = collection;
        }

        public async Task<string> Create(BloodEvent bloodEvent)
        {
            await _bloodEvent.InsertOneAsync(bloodEvent);
            return bloodEvent._id;
        }

        public Task<BloodEvent> Get(string _id)
        {
            var filter = Builders<BloodEvent>.Filter.Eq(e => e._id, _id);
            var bloodEvent = _bloodEvent.Find(filter).FirstOrDefaultAsync();

            return bloodEvent;
        }

        public async Task<IEnumerable<BloodEvent>> Get()
        {
            var bloodEvent = await _bloodEvent.Find(_ => true).ToListAsync();
            return bloodEvent;
        }

        public async Task<bool> Update(string _id, BloodEvent bloodEvent)
        {
            var filter = Builders<BloodEvent>.Filter.Eq(e => e._id, _id);
            var update = Builders<BloodEvent>.Update
                .Set(e => e.event_name, bloodEvent.event_name)
                .Set(e => e.location, bloodEvent.location)
                .Set(e => e.date, bloodEvent.date);

            var result = await _bloodEvent.UpdateOneAsync(filter, update);

            return result.ModifiedCount == 1;
        }

        public async Task<bool> Delete(string _id)
        {
            var filter = Builders<BloodEvent>.Filter.Eq(e => e._id, _id);
            var result = await _bloodEvent.DeleteOneAsync(filter);

            return result.DeletedCount == 1;
        }
    }
}