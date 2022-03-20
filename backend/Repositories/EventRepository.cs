using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly IMongoCollection<Event> _event;

        public EventRepository(IMongoClient client)
        {
            var database = client.GetDatabase("BloodBank");
            var collection = database.GetCollection<Event>(nameof(Event));

            _event = collection;
        }

        public async Task<string> Create(Event e)
        {
            await _event.InsertOneAsync(e);
            return e._id;
        }

        public Task<Event> Get(string _id)
        {
            var filter = Builders<Event>.Filter.Eq(e => e._id, _id);
            var e = _event.Find(filter).FirstOrDefaultAsync();

            return e;
        }

        public async Task<IEnumerable<Event>> Get()
        {
            var e = await _event.Find(_ => true).ToListAsync();
            return e;
        }

        public async Task<bool> Update(string _id, Event e)
        {
            var filter = Builders<Event>.Filter.Eq(e => e._id, _id);
            var update = Builders<Event>.Update
                .Set(e => e.name, e.name)
                .Set(e => e.location, e.location)
                .Set(e => e.date, e.date);

            var result = await _event.UpdateOneAsync(filter, update);

            return result.ModifiedCount == 1;
        }

        public async Task<bool> Delete(string _id)
        {
            var filter = Builders<Event>.Filter.Eq(e => e._id, _id);
            var result = await _event.DeleteOneAsync(filter);

            return result.DeletedCount == 1;
        }
    }
}