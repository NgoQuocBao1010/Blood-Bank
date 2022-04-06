using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly IMongoCollection<Request> _request;
        private readonly IHospitalRepository _hospitalRepository;

        public RequestRepository(IMongoClient client)
        {
            _hospitalRepository = new HospitalRepository(client);
            var database = client.GetDatabase("BloodBank");
            var collection = database.GetCollection<Request>(nameof(Request));
            
            _request = collection;
            AddDefaultData();
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

        public async Task<IEnumerable<Request>> GetRequestByHospitalId(string hospitalId)
        {
            var filter = Builders<Request>.Filter.Eq(r => r.HospitalId, hospitalId);
            var request = await _request.Find(filter).ToListAsync();
            return request;
        }

        public async Task<IEnumerable<Request>> GetPendingRequest()
        {
            var filter = Builders<Request>.Filter.Eq(r => r.Status, 0);
            var request = await _request.Find(filter).ToListAsync();
            return request;
        }

        public async Task<IEnumerable<Request>> GetApprovedRequest()
        {
            var filter = Builders<Request>.Filter.Eq(r => r.Status, 1);
            var request = await _request.Find(filter).ToListAsync();
            return request;
        }

        public async Task<IEnumerable<Request>> GetRejectedRequest()
        {
            var filter = Builders<Request>.Filter.Eq(r => r.Status, -1);
            var request = await _request.Find(filter).ToListAsync();
            return request;
        }

        public async Task<bool> Update(string _id, Request request)
        {
            var filter = Builders<Request>.Filter.Eq(r => r._id, _id);
            var update = Builders<Request>.Update
                .Set(r => r.Date, request.Date)
                .Set(r => r.Quantity, request.Quantity)
                .Set(r => r.Blood.Name, request.Blood.Name)
                .Set(r => r.Blood.Type, request.Blood.Type)
                .Set(r => r.HospitalId, request.HospitalId)
                .Set(r => r.HospitalName, request.HospitalName)
                .Set(r => r.Status, request.Status)
                .Set(r => r.RejectReason, request.RejectReason);

            var result = await _request.UpdateOneAsync(filter, update);

            return result.ModifiedCount == 1;
        }

        public async Task<bool> Delete(string _id)
        {
            var filter = Builders<Request>.Filter.Eq(r => r._id, _id);
            var result = await _request.DeleteOneAsync(filter);
            return result.DeletedCount == 1;
        }

        public void AddDefaultData()
        {
            var requests = Get();
            if (requests.Result.Any()) return;
            var hospitals = _hospitalRepository.Get();

            var listRequest = new List<Request>();
            if (hospitals != null)
            {
                var listHospital = hospitals.Result.ToList();
                for (var i = 0; i < listHospital.Count; i++)
                {
                    var requestBlood = new RequestBlood("A", "Negative");
                    var request = new Request("1614976400000", 4000, requestBlood, listHospital[i]._id,
                        listHospital[i].Name, 0, "");
                    switch (i)
                    {
                        case 1:
                            requestBlood = new RequestBlood("AB", "Positive");
                            request = new Request("1648659600000", 7000, requestBlood, listHospital[i]._id,
                                listHospital[i].Name, 0, "");
                            break;
                        case 2:
                            requestBlood = new RequestBlood("O", "Negative");
                            request = new Request("1649091600000", 10000, requestBlood, listHospital[i]._id,
                                listHospital[i].Name, 0, "");
                            break;
                    }

                    listRequest.Add(request);
                }
            }

            _request.InsertMany(listRequest);
        }
        
        public async void ApproveRequest(Request request)
        {
            var filter = Builders<Request>.Filter.Eq(r => r._id, request._id);
            var update = Builders<Request>.Update.Set(r => r.Status, request.Status = 1);
            await _request.UpdateOneAsync(filter, update);
        }

        public async void RejectRequest(Request request)
        {
            var filter = Builders<Request>.Filter.Eq(r => r._id, request._id);
            var update = Builders<Request>.Update.Set(r => r.Status, request.Status = -1)
                .Set(r => r.RejectReason, request.RejectReason);
            await _request.UpdateOneAsync(filter, update);
        }
        
        public async Task<IEnumerable<Request>> GetRequestByStatus(int status)
        {
            var filter = Builders<Request>.Filter.Eq(r => r.Status, status);
            var request = await _request.Find(filter).ToListAsync();

            return request;
        }
    }
}