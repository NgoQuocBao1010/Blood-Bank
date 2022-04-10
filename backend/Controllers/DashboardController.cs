using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDonorRepository _donorRepository;
        private readonly IDonorTransactionRepository _donorTransactionRepository;
        private readonly IRequestRepository _requestRepository;

        public DashboardController(IDonorRepository donorRepository, IRequestRepository requestRepository,
            IDonorTransactionRepository donorTransactionRepository)
        {
            _donorRepository = donorRepository;
            _requestRepository = requestRepository;
            _donorTransactionRepository = donorTransactionRepository;
        }

        [HttpGet]
        [Route("api/recentActivities")]
        public async Task<IActionResult> GetRecentActivities()
        {
            var result = new List<RecentActivities>();
            var listApprovedTransactions = await _donorTransactionRepository.GetTransactionByStatus(1);
            var listId = listApprovedTransactions
                .Select(transaction => new Id(transaction.donorId, transaction.updateStatusAt, "transaction", transaction.amount)).ToList();

            var listApprovedRequest = await _requestRepository.GetRequestByStatus(1);
            listId.AddRange(listApprovedRequest.Select(request => new Id(request._id, request.updateStatusAt, "request", request.Quantity)));

            listId = new List<Id>(listId.OrderByDescending(id => long.Parse(id.date)));

            foreach (var id in listId)
            {
                RecentActivities recentActivity = null;
                switch (id.type)
                {
                    case "transaction":
                        var donor = await _donorRepository.Get(id._id);
                        recentActivity = new RecentActivities(id._id, "Receive", donor.name,
                            id.date, id.amount);
                        break;
                    case "request":
                        var request = await _requestRepository.Get(id._id);
                        recentActivity = new RecentActivities(id._id, "Donate", request.HospitalName,
                            id.date, id.amount);
                        break;
                }
                result.Add(recentActivity);
                if (result.Count == 5)
                {
                    break;
                }
            }

            return new JsonResult(result);
        }
    }
}