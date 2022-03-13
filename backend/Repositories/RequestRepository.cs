using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly IMongoCollection<Request> _request;

        public RequestRepository(IMongoClient client)
        {
            var database = client.GetDatabase("Request");
            var collection = database.GetCollection<Request>(nameof(Request));

            _request = collection;
        }

        public async Task<string> Create(Request request)
        {
            await _request.InsertOneAsync(request);
            return request._id;
        }

        public Task<Request> Get(string _id)
        {
            var filter = Builders<Request>.Filter.Eq(r => r._id, _id);
            var request = _request.Find(filter).FirstOrDefaultAsync();
            return request;
        }

        public async Task<IEnumerable<Request>> Get()
        {
            var request = await _request.Find(_ => true).ToListAsync();
            return request;
        }

        public async Task<bool> Update(string _id, Request request)
        {
            var filter = Builders<Request>.Filter.Eq(r => r._id, _id);
            var update = Builders<Request>.Update
                .Set(r => r.blood_id, request.blood_id)
                .Set(r => r.volume, request.volume)
                .Set(r => r.hospital_id, request.hospital_id);
            
            var result = await _request.UpdateOneAsync(filter, update);
            
            return result.ModifiedCount == 1;
        }

        public async Task<bool> Delete(string _id)
        {
            var filter = Builders<Request>.Filter.Eq(r => r._id, _id);
            var result = await _request.DeleteOneAsync(filter);
            return result.DeletedCount == 1;
        }
    }
}