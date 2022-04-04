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
    [Route("api")]
    [Authorize(Roles = "admin")]
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
        [Route("recentActivities")]
        public async Task<IActionResult> GetRecentActivities()
        {
            var result = new List<RecentActivities>();
            var listApprovedTransactions = await _donorTransactionRepository.GetTransactionByStatus(1);
            var listId = listApprovedTransactions
                .Select(transaction => new Id(transaction.donorId, transaction.dateDonated, "transaction")).ToList();

            var listApprovedRequest = await _requestRepository.GetRequestByStatus(1);
            listId.AddRange(listApprovedRequest.Select(request => new Id(request._id, request.Date, "request")));

            listId = new List<Id>(listId.OrderByDescending(id => long.Parse(id.date)));

            foreach (var id in listId)
            {
                RecentActivities recentActivity = null;
                switch (id.type)
                {
                    case "transaction":
                        var donor = await _donorRepository.Get(id._id);
                        recentActivity = new RecentActivities(id._id, "Blood Receive", donor.name,
                            id.date);
                        break;
                    case "request":
                        var request = await _requestRepository.Get(id._id);
                        recentActivity = new RecentActivities(id._id, "Blood Donated", request.HospitalName,
                            id.date);
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

        [HttpGet]
        [Route("utilities")]
        public async Task<IActionResult> GetUtilities()
        {
            var totalBloodReceive = await GetTotalBloodReceive();
            return new JsonResult(totalBloodReceive);
        }
        
        public async Task<double> GetTotalBloodReceive()
        {
            var transactions = await _donorTransactionRepository.GetTransactionByStatus(1);
            double total = transactions.Sum(transaction => transaction.amount);
            return total;
        }
        
        public async Task<double> GetBloodReceiveLastQuarter()
        {
            var transactions = await _donorTransactionRepository.GetTransactionByStatus(1);
            double total = transactions.Sum(transaction => transaction.amount);
            return total;
        }
    }
}