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

            var listEvent = new List<Event>
            {
                new("Health and Wellbeing at work", new Location("Cần Thơ", "F+"), "1612976400000",
                    2, "This is a blood donation event at F+"),
                new("Tell me, do you bleed?", new Location("Hậu Giang", "Nga 6"), "1639069200000",
                    2, "This is a blood donation event at Nga 6"),
                new("We are donors", new Location("Hồ Chí Minh", "Cho Ray"), "1646067600000",
                    2, "This is a blood donation event at Cho Ray"),
                new("Judoh Blood Donations", new Location("An Giang", "Nha Cua May"), "1644080400000",
                    2, "This is a blood donation event at Nha Cua May"),
                new("Judoh Blood Donations - Summer Edition", new Location("Đà Lạt", "Nomad Homestay"), "1644080400000",
                    100, "This is a blood donation event at Nomad"),
                new("Judoh Blood Donations - Chrismas Edition", new Location("Cần Thơ", "Cafe Station"),
                    "1671469200000",
                    2, "This is a blood donation event at Cafe Station")
            };

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
            var sortEvent = e.OrderByDescending(x => long.Parse(x.startDate));
            return sortEvent;
        }

        public async Task<long> Update(string _id, Event e)
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

            return result.ModifiedCount;
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