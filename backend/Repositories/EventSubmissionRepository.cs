using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Repositories
{
    public class EventSubmissionRepository : IEventSubmissionRepository
    {
        private readonly IMongoCollection<EventSubmission> _eventSubmission;
        private readonly IEventRepository _eventRepository;

        public EventSubmissionRepository(IMongoClient client)
        {
            _eventRepository = new EventRepository(client);
            var database = client.GetDatabase("BloodBank");
            var collection = database.GetCollection<EventSubmission>(nameof(EventSubmission));

            _eventSubmission = collection;
            AddDefaultData();
        }
        
        public void AddDefaultData()
        {
            _eventRepository.AddDefaultData();
            // Get the first eventId from default data.
            var firstEvent = _eventRepository.Get();
            var eventId = firstEvent.Result.First()._id;

            var eventSubmission = Get();
            if (eventSubmission.Result.Any()) return;

            var listEventSubmission = new List<EventSubmission>
            {
                new(eventId,
                    "093201234567",
                    "Trương Hoàng Thuận",
                    "0123456789",
                    "thuan@gmail.com",
                    "Cần Thơ",
                    "male",
                    "973468800",
                    "1640390400000"),
                new(eventId,
                    "093212345678",
                    "Ngô Hồng Quốc Bảo",
                    "1234567890",
                    "bao@gmail.com",
                    "Cần Thơ",
                    "male",
                    "971136000",
                    "1640390400000"),
                new(eventId,
                    "093223456789",
                    "Bùi Quốc Trọng",
                    "2345678901",
                    "trong@gmail.com",
                    "Hồ Chí Minh",
                    "male",
                    "958003200",
                    "1640390400000"),
                new(eventId,
                    "0932345678912",
                    "Lê Chánh Nhựt",
                    "3456789012",
                    "nhut@gmail.com",
                    "Cần Thơ",
                    "male",
                    "949881600",
                    "1640390400000")
            };
            
            _eventSubmission.InsertMany(listEventSubmission);
        }

        public async Task<string> Create(EventSubmission eventSubmission)
        {
            await _eventSubmission.InsertOneAsync(eventSubmission);
            return eventSubmission._id;
        }

        public Task<EventSubmission> Get(string _id)
        {
            var filter = Builders<EventSubmission>.Filter.Eq(es => es._id, _id);
            var request = _eventSubmission.Find(filter).FirstOrDefaultAsync();
            return request;
        }

        public async Task<IEnumerable<EventSubmission>> Get()
        {
            var request = await _eventSubmission.Find(_ => true).ToListAsync();
            return request;
        }
        
        public Task<List<EventSubmission>> GetByEvent(string eventId)
        {
            var filter = Builders<EventSubmission>.Filter.Eq(es => es.EventId, eventId);
            var eventSubmission = _eventSubmission.Find(filter).ToListAsync();
            return eventSubmission;
        }

        public async Task<bool> Update(string _id, EventSubmission eventSubmission)
        {
            var filter = Builders<EventSubmission>.Filter.Eq(es => es._id, _id);
            var update = Builders<EventSubmission>.Update
                .Set(es => es._id, eventSubmission._id)
                .Set(es => es.EventId, eventSubmission.EventId)
                .Set(es => es.IdCardNumber, eventSubmission.IdCardNumber)
                .Set(es => es.FullName, eventSubmission.FullName)
                .Set(es => es.Phone, eventSubmission.Phone)
                .Set(es => es.Address, eventSubmission.Address)
                .Set(es => es.Email, eventSubmission.Email)
                .Set(es => es.Gender, eventSubmission.Gender)
                .Set(es => es.Dob, eventSubmission.Dob)
                .Set(es => es.LatestDonationDate, eventSubmission.LatestDonationDate);
            
            var result = await _eventSubmission.UpdateOneAsync(filter, update);
            
            return result.ModifiedCount == 1;
        }

        public async Task<bool> Delete(string _id)
        {
            var filter = Builders<EventSubmission>.Filter.Eq(es => es._id, _id);
            var result = await _eventSubmission.DeleteOneAsync(filter);
            return result.DeletedCount == 1;
        }
    }
}