using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Repositories
{
    public class EventSubmissionRepository : IEventSubmissionRepository
    {
        private readonly IMongoCollection<EventSubmission> _eventSubmission;

        public EventSubmissionRepository(IMongoClient client)
        {
            var database = client.GetDatabase("BloodBank");
            var collection = database.GetCollection<EventSubmission>(nameof(EventSubmission));

            _eventSubmission = collection;
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

        public async Task<bool> Update(string _id, EventSubmission eventSubmission)
        {
            var filter = Builders<EventSubmission>.Filter.Eq(es => es._id, _id);
            var update = Builders<EventSubmission>.Update
                .Set(es => es._id, eventSubmission._id)
                .Set(es => es.EventId, eventSubmission.EventId)
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