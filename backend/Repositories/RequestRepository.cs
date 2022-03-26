using System.Collections.Generic;
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
            var database = client.GetDatabase("BloodBank");
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
                .Set(r => r.Date, request.Date)
                .Set(r => r.RequestQuantity, request.RequestQuantity)
                .Set(r => r.RequestBlood.Name, request.RequestBlood.Name)
                .Set(r => r.RequestBlood.Type, request.RequestBlood.Type)
                .Set(r => r.RequestHospital._id, request.RequestHospital._id)
                .Set(r => r.RequestHospital.Name, request.RequestHospital.Name);
            
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