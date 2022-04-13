using System.Collections.Generic;
using System.Linq;
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
            AddDefaultData();
        }

        public void AddDefaultData()
        {
            var e = Get();
            if (e.Result.Any()) return;
            
            var data = DefaultData.ReadJson();
            foreach (var events in data.Result.Events)
            {
                events.binaryImage = null;
            }
            _event.InsertMany(data.Result.Events);
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
            var sortEvent = e.OrderByDescending(x => long.Parse(x.startDate));
            return sortEvent;
        }

        public async Task<Event> Update(string _id, Event e)
        {
            var filter = Builders<Event>.Filter.Eq(events => events._id, _id);
            var update = Builders<Event>.Update
                .Set(events => events.name, e.name)
                .Set(events => events.location.city, e.location.city)
                .Set(events => events.location.address, e.location.address)
                .Set(events => events.startDate, e.startDate)
                .Set(events => events.duration, e.duration)
                .Set(events => events.detail, e.detail)
                .Set(events => events.binaryImage, e.binaryImage);

            var result = await _event.UpdateOneAsync(filter, update);

            e._id = _id;
            return e;
        }

        public async Task<bool> UpdateParticipant(string _id, int numOfParticipants)
        {
            var filter = Builders<Event>.Filter.Eq(events => events._id, _id);
            var update = Builders<Event>.Update
                .Set(events => events.participants, numOfParticipants);
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