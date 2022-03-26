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
        
        public void AddDefaultData(IEnumerable<Event> listEvent)
        {
            _event.InsertMany(listEvent);
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
            var filter = Builders<Event>.Filter.Eq(events => events._id, _id);
            var update = Builders<Event>.Update
                .Set(events => events.name, e.name)
                .Set(events => events.location.city, e.location.city)
                .Set(events => events.location.address, e.location.address)
                .Set(events => events.startDate, e.startDate)
                .Set(events => events.duration, e.duration)
                .Set(events => events.detail, e.detail);

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