using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IEventRepository _eventRepository;

        public DashboardController(IDonorRepository donorRepository, IRequestRepository requestRepository,
            IDonorTransactionRepository donorTransactionRepository, IEventRepository eventRepository)
        {
            _donorRepository = donorRepository;
            _requestRepository = requestRepository;
            _donorTransactionRepository = donorTransactionRepository;
            _eventRepository = eventRepository;
        }

        [HttpGet]
        [Route("recentActivities")]
        public async Task<IActionResult> GetRecentActivities()
        {
            var result = new List<RecentActivities>();
            var listApprovedTransactions = await _donorTransactionRepository.GetTransactionByStatus(1);
            var listId = listApprovedTransactions
                .Select(transaction =>
                    new Id(transaction.donorId, transaction.dateDonated, "transaction", transaction._id)).ToList();

            var listApprovedRequest = await _requestRepository.GetRequestByStatus(1);
            listId.AddRange(listApprovedRequest.Select(request => new Id(request._id, request.Date, "request", null)));

            listId = new List<Id>(listId.OrderByDescending(id => long.Parse(id.date)));

            foreach (var id in listId)
            {
                RecentActivities recentActivity = null;
                switch (id.type)
                {
                    case "transaction":
                        var donor = await _donorRepository.Get(id._id);
                        recentActivity = new RecentActivities(id._id, "receive", donor.name,
                            id.date, id.transactionId);
                        break;
                    case "request":
                        var request = await _requestRepository.Get(id._id);
                        recentActivity = new RecentActivities(id._id, "donate", request.HospitalName,
                            id.date, null);
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
        [Route("dashboardInfo")]
        public async Task<IActionResult> GetDashboardInfo()
        {
            // get current time
            var currentUnixTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            // convert to current date
            var currentDate = DateTimeOffset.FromUnixTimeMilliseconds(currentUnixTime).DateTime;
            // get an index of quarter
            var quarterNumber = (currentDate.Month - 1) / 3 + 1;
            // get first day of current quarter
            DateTimeOffset firstDayOfCurrentQuarter = new DateTime(currentDate.Year, (quarterNumber - 1) * 3 + 1, 1);
            // get last day of current quarter
            var lastDayOfCurrentQuarter = firstDayOfCurrentQuarter.AddMonths(3).AddDays(-1);
            // get first day of last quarter
            var firstDayOfLastQuarter = firstDayOfCurrentQuarter.AddMonths(-3);
            if (quarterNumber == 1)
            {
                firstDayOfLastQuarter = firstDayOfCurrentQuarter.AddYears(-1).AddMonths(-3);
            }

            // get last day of last quarter
            var lastDayOfLastQuarter = firstDayOfLastQuarter.AddMonths(3).AddDays(-1);

            // get total and percentage of blood receive when compare to last quarter
            var totalBloodReceive = await GetTotalBloodReceive();
            var bloodReceiveLastQuarter = await GetBloodReceiveLastQuarter(
                firstDayOfLastQuarter.ToUnixTimeMilliseconds(),
                lastDayOfLastQuarter.ToUnixTimeMilliseconds(), firstDayOfCurrentQuarter.ToUnixTimeMilliseconds());
            var bloodReceive = new BloodReceive(totalBloodReceive, bloodReceiveLastQuarter);

            // get total and percentage of blood donated when compare to last quarter
            var totalBloodDonated = await GetTotalBloodDonated();
            var bloodDonatedLastQuarter = await GetBloodDonatedLastQuarter(
                firstDayOfLastQuarter.ToUnixTimeMilliseconds(),
                lastDayOfLastQuarter.ToUnixTimeMilliseconds(), firstDayOfCurrentQuarter.ToUnixTimeMilliseconds());
            var bloodDonated = new BloodDonated(totalBloodDonated, bloodDonatedLastQuarter);

            // get total and percentage of donators when compare to last quarter
            var totalDonators = await GetTotalDonators();
            var donatorsLastQuarter = await GetDonatorsLastQuarter(
                firstDayOfLastQuarter.ToUnixTimeMilliseconds(),
                lastDayOfLastQuarter.ToUnixTimeMilliseconds(), firstDayOfCurrentQuarter.ToUnixTimeMilliseconds());
            var donators = new Donators(totalDonators.Count, donatorsLastQuarter);

            // get total and percentage of events when compare to last quarter
            var totalEvents = await GetTotalEvents();
            var eventsLastQuarter = await GetEventsLastQuarter(
                firstDayOfLastQuarter.ToUnixTimeMilliseconds(),
                lastDayOfLastQuarter.ToUnixTimeMilliseconds(), firstDayOfCurrentQuarter.ToUnixTimeMilliseconds(),
                lastDayOfCurrentQuarter.ToUnixTimeMilliseconds());
            var events = new Events(totalEvents, eventsLastQuarter);

            var dashboardInfo = new DashboardInfo(bloodReceive, bloodDonated, donators, events);

            return new JsonResult(dashboardInfo);
        }

        public async Task<float> GetTotalBloodReceive()
        {
            var transactions = await _donorTransactionRepository.GetTransactionByStatus(1);
            float total = transactions.Sum(transaction => transaction.amount);
            return total;
        }

        public async Task<float> GetBloodReceiveLastQuarter(long firstDayOfLastQuarter, long lastDayOfLastQuarter,
            long firstDayOfCurrentQuarter)
        {
            var transactions = await _donorTransactionRepository.GetTransactionByStatus(1);
            float bloodReceiveLastQuarter = 0;
            float bloodReceiveCurrentQuarter = 0;

            foreach (var transaction in transactions)
            {
                var date = long.Parse(transaction.dateDonated);
                if (date >= firstDayOfLastQuarter && date <= lastDayOfLastQuarter)
                {
                    bloodReceiveLastQuarter += transaction.amount;
                }

                if (date >= firstDayOfCurrentQuarter)
                {
                    bloodReceiveCurrentQuarter += transaction.amount;
                }
            }

            if (Math.Abs(bloodReceiveLastQuarter - bloodReceiveCurrentQuarter) == 0)
            {
                return 0;
            }

            if (bloodReceiveCurrentQuarter == 0)
            {
                return -1;
            }

            if (bloodReceiveLastQuarter == 0)
            {
                return 1;
            }

            var percent = bloodReceiveCurrentQuarter / bloodReceiveLastQuarter - 1;

            return (float) Math.Round(percent, 2);
        }

        public async Task<float> GetTotalBloodDonated()
        {
            var requests = await _requestRepository.GetRequestByStatus(1);
            float total = requests.Sum(request => request.Quantity);
            return total;
        }

        public async Task<float> GetBloodDonatedLastQuarter(long firstDayOfLastQuarter, long lastDayOfLastQuarter,
            long firstDayOfCurrentQuarter)
        {
            var requests = await _requestRepository.GetRequestByStatus(1);
            float bloodDonatedLastQuarter = 0;
            float bloodDonatedCurrentQuarter = 0;

            foreach (var request in requests)
            {
                var date = long.Parse(request.Date);
                if (date >= firstDayOfLastQuarter && date <= lastDayOfLastQuarter)
                {
                    bloodDonatedLastQuarter += request.Quantity;
                }

                if (date >= firstDayOfCurrentQuarter)
                {
                    bloodDonatedCurrentQuarter += request.Quantity;
                }
            }

            if (Math.Abs(bloodDonatedLastQuarter - bloodDonatedCurrentQuarter) == 0)
            {
                return 0;
            }

            if (bloodDonatedCurrentQuarter == 0)
            {
                return -1;
            }

            if (bloodDonatedLastQuarter == 0)
            {
                return 1;
            }

            var percent = bloodDonatedCurrentQuarter / bloodDonatedLastQuarter - 1;

            return (float) Math.Round(percent, 2);
        }

        public async Task<List<string>> GetTotalDonators()
        {
            var listTransaction = new List<string>();
            var transactions = await _donorTransactionRepository.GetTransactionByStatus(1);
            foreach (var transaction in transactions)
            {
                if (!listTransaction.Contains(transaction.donorId))
                {
                    listTransaction.Add(transaction.donorId);
                }
            }

            return listTransaction;
        }

        public async Task<int> GetDonatorsLastQuarter(long firstDayOfLastQuarter, long lastDayOfLastQuarter,
            long firstDayOfCurrentQuarter)
        {
            var listDonatorsLastQuarter = new List<string>();
            var listDonatorsCurrentQuarter = new List<string>();
            var listDonor = new List<string>();
            var transactions = await _donorTransactionRepository.GetTransactionByStatus(1);

            var donorTransactions = transactions.ToList();
            foreach (var transaction in donorTransactions)
            {
                var date = long.Parse(transaction.dateDonated);
                if (date < firstDayOfLastQuarter && !listDonor.Contains(transaction.donorId))
                {
                    listDonor.Add(transaction.donorId);
                }
            }

            foreach (var transaction in donorTransactions)
            {
                var date = long.Parse(transaction.dateDonated);
                if (date >= firstDayOfLastQuarter && date <= lastDayOfLastQuarter &&
                    !listDonatorsLastQuarter.Contains(transaction.donorId) && !listDonor.Contains(transaction.donorId))
                {
                    listDonatorsLastQuarter.Add(transaction.donorId);
                }

                if (date >= firstDayOfCurrentQuarter && !listDonatorsCurrentQuarter.Contains(transaction.donorId) &&
                    !listDonatorsLastQuarter.Contains(transaction.donorId) && !listDonor.Contains(transaction.donorId))
                {
                    listDonatorsCurrentQuarter.Add(transaction.donorId);
                }
            }


            return listDonatorsCurrentQuarter.Count - listDonatorsLastQuarter.Count;
        }

        public async Task<int> GetTotalEvents()
        {
            var events = await _eventRepository.Get();
            return events.Count();
        }

        public async Task<int> GetEventsLastQuarter(long firstDayOfLastQuarter, long lastDayOfLastQuarter,
            long firstDayOfCurrentQuarter, long lastDayOfCurrentQuarter)
        {
            var events = await _eventRepository.Get();
            var listEvents = events.ToList();
            var eventsLastQuarter = listEvents.Select(e => long.Parse(e.startDate))
                .Count(date => date >= firstDayOfLastQuarter && date <= lastDayOfLastQuarter);
            var eventsCurrentQuarter = listEvents.Select(e => long.Parse(e.startDate))
                .Count(date => date >= firstDayOfCurrentQuarter && date <= lastDayOfCurrentQuarter);

            return eventsCurrentQuarter - eventsLastQuarter;
        }
    }
}