using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Repositories
{
    public class RecentActivityRepository : IRecentActivityRepository
    {
        private readonly IMongoCollection<RecentActivity> _recentActivity;
        private readonly IDonorRepository _donorRepository;
        private readonly IDonorTransactionRepository _donorTransactionRepository;
        private readonly IRequestRepository _requestRepository;

        public RecentActivityRepository(IMongoClient client)
        {
            _donorRepository = new DonorRepository(client);
            _donorTransactionRepository = new DonorTransactionRepository(client);
            _requestRepository = new RequestRepository(client);
            var database = client.GetDatabase("BloodBank");
            var collection = database.GetCollection<RecentActivity>(nameof(RecentActivity));

            _recentActivity = collection;
            AddDefaultData();
        }

        public async Task<string> Create(RecentActivity recentActivity)
        {
            await _recentActivity.InsertOneAsync(recentActivity);
            return recentActivity._id;
        }

        public void AddDefaultData()
        {
            var listActivity = Get();
            if (listActivity.Result.Any()) return;
            var listRecentActivity = new List<RecentActivity>();
            var listApprovedTransactions = _donorTransactionRepository.GetTransactionByStatus(1);
            var listId = listApprovedTransactions.Result.Select(transaction => new Id(transaction.donorId,
                transaction.updateStatusAt, "transaction", transaction._id, transaction.amount)).ToList();

            var listApprovedRequest = _requestRepository.GetRequestByStatus(1);
            listId.AddRange(listApprovedRequest.Result.Select(request =>
                new Id(request.HospitalId, request.updateStatusAt, "request", request._id, request.Quantity)));

            listId = new List<Id>(listId.OrderByDescending(id => long.Parse(id.date)));

            foreach (var id in listId)
            {
                RecentActivity recentActivity = null;
                switch (id.type)
                {
                    case "transaction":
                        var donor = _donorRepository.Get(id._id);
                        recentActivity = new RecentActivity("Donor", id._id, id.transactionId,
                            "plus", donor.Result.name, id.date, id.amount);
                        break;
                    case "request":
                        var request = _requestRepository.Get(id.transactionId);
                        recentActivity = new RecentActivity("Hospital", id._id, id.transactionId,
                            "minus", request.Result.HospitalName, id.date, id.amount);
                        break;
                }

                listRecentActivity.Add(recentActivity);
            }

            _recentActivity.InsertMany(listRecentActivity);
        }

        public async Task<IEnumerable<RecentActivity>> Get()
        {
            var recentActivities = await _recentActivity.Find(_ => true).ToListAsync();
            var sortActivities = recentActivities.OrderByDescending(r => Convert.ToInt64(r.date));
            return sortActivities;
        }
    }
}