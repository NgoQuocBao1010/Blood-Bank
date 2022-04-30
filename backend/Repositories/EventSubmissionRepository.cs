using System;
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
            var e = _eventRepository.Get();
            var listEvent = e.Result.ToList();

            var eventSubmission = Get();
            if (eventSubmission.Result.Any()) return;

            var data = DefaultData.ReadJson();
            foreach (var submission in data.Result.EventSubmissions)
            {
                submission.EventId = listEvent[^int.Parse(submission.EventId)]._id;
            }

            _eventSubmission.InsertMany(data.Result.EventSubmissions);
        }

        public async Task<string> Create(EventSubmission eventSubmission)
        {
            await _eventSubmission.InsertOneAsync(eventSubmission);
            return eventSubmission._id;
        }

        public Task<EventSubmission> Get(string _id)
        {
            var filter = Builders<EventSubmission>.Filter.Eq(es => es._id, _id);
            var submission = _eventSubmission.Find(filter).FirstOrDefaultAsync();
            return submission;
        }

        public async Task<IEnumerable<EventSubmission>> Get()
        {
            var submission = await _eventSubmission.Find(_ => true).ToListAsync();
            var sortEventSubmission = submission.OrderByDescending(s => long.Parse(s.DateSubmitted));
            return sortEventSubmission;
        }

        public Task<List<EventSubmission>> GetByEvent(string eventId)
        {
            var filter = Builders<EventSubmission>.Filter.Eq(es => es.EventId, eventId);
            var eventSubmission = _eventSubmission.Find(filter).ToListAsync();
            return eventSubmission;
        }

        public async Task<IEnumerable<Report>> GetLast2Days(long today, long yesterday)
        {
            var filter = Builders<EventSubmission>.Filter.Gte(e => e.DateSubmitted, yesterday.ToString())
                         & Builders<EventSubmission>.Filter.Lte(e => e.DateSubmitted, today.ToString());
            var listEventSub = await _eventSubmission.Find(filter).ToListAsync();
            var listReport = listEventSub
                .Select(e => new Report(e.EventId, "Event Submission", e.FullName, null, e.DateSubmitted))
                .ToList();

            return listReport;
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