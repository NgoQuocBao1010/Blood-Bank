using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver.Linq;

namespace backend.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api")]
    [Authorize(Roles = "admin")]
    public class DashboardController : ControllerBase
    {
        private readonly IDonorRepository _donorRepository;
        private readonly IDonorTransactionRepository _donorTransactionRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventSubmissionRepository _eventSubmissionRepository;
        private readonly IRecentActivityRepository _recentActivityRepository;

        public DashboardController(IDonorRepository donorRepository, IRequestRepository requestRepository,
            IDonorTransactionRepository donorTransactionRepository, IEventRepository eventRepository,
            IRecentActivityRepository recentActivityRepository, IEventSubmissionRepository eventSubmissionRepository)
        {
            _donorRepository = donorRepository;
            _requestRepository = requestRepository;
            _donorTransactionRepository = donorTransactionRepository;
            _eventRepository = eventRepository;
            _eventSubmissionRepository = eventSubmissionRepository;
            _recentActivityRepository = recentActivityRepository;
        }


        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("recentActivities")]
        public async Task<IActionResult> GetRecentActivities()
        {
            var recentActivities = await _recentActivityRepository.Get();
            var listActivity = recentActivities.ToList();
            var result = new List<RecentActivity>();
            for (var i = 0; i < 5; i++)
            {
                result.Add(listActivity[i]);
            }

            return new JsonResult(result);
        }

        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("dashboardInfo")]
        public async Task<IActionResult> GetDashboardInfo()
        {
            // get current time
            var currentUnixTime = DateTimeOffset.Now.ToLocalTime().ToUnixTimeMilliseconds();
            // convert to current date
            var currentDate = DateTimeOffset.FromUnixTimeMilliseconds(currentUnixTime).DateTime.ToLocalTime();
            // get an index of quarter
            var quarterNumber = (currentDate.Month - 1) / 3 + 1;
            // get first day of current quarter
            DateTimeOffset firstDayOfCurrentQuarter =
                new DateTime(currentDate.Year, (quarterNumber - 1) * 3 + 1, 1).ToLocalTime();
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
                lastDayOfLastQuarter.ToUnixTimeMilliseconds(), firstDayOfCurrentQuarter.ToUnixTimeMilliseconds(),
                lastDayOfCurrentQuarter.ToUnixTimeMilliseconds());
            var bloodReceive = new BloodReceive(totalBloodReceive, bloodReceiveLastQuarter);

            // get total and percentage of blood donated when compare to last quarter
            var totalBloodDonated = await GetTotalBloodDonated();
            var bloodDonatedLastQuarter = await GetBloodDonatedLastQuarter(
                firstDayOfLastQuarter.ToUnixTimeMilliseconds(),
                lastDayOfLastQuarter.ToUnixTimeMilliseconds(), firstDayOfCurrentQuarter.ToUnixTimeMilliseconds(),
                lastDayOfCurrentQuarter.ToUnixTimeMilliseconds());
            var bloodDonated = new BloodDonated(totalBloodDonated, bloodDonatedLastQuarter);

            // get total and percentage of donators when compare to last quarter
            var totalDonators = await GetTotalDonators();
            var donatorsLastQuarter = await GetDonatorsLastQuarter(
                firstDayOfLastQuarter.ToUnixTimeMilliseconds(),
                lastDayOfLastQuarter.ToUnixTimeMilliseconds(), firstDayOfCurrentQuarter.ToUnixTimeMilliseconds(),
                lastDayOfCurrentQuarter.ToUnixTimeMilliseconds());
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
            long firstDayOfCurrentQuarter, long lastDayOfCurrentQuarter)
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

                if (date >= firstDayOfCurrentQuarter && date <= lastDayOfCurrentQuarter)
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
            long firstDayOfCurrentQuarter, long lastDayOfCurrentQuarter)
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

                if (date >= firstDayOfCurrentQuarter && date <= lastDayOfCurrentQuarter)
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
            long firstDayOfCurrentQuarter, long lastDayOfCurrentQuarter)
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

                if (date >= firstDayOfCurrentQuarter && date <= lastDayOfCurrentQuarter &&
                    !listDonatorsCurrentQuarter.Contains(transaction.donorId) &&
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

        /*
        Function to convert string epoch to DateTime yyyy-mm-dd
        and return string epoch for that DateTime.
        */

        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("notification")]
        public async Task<IActionResult> GetDashboardNotification()
        {
            // try
            // {
                var notification = new Notifications();
                var listReportToday = new List<Report>();
                var listReportYesterday = new List<Report>();
                var today = DateTime.Today.ToLocalTime();
                var todayStart = new DateTimeOffset(today).ToUnixTimeMilliseconds();
                var unixToday = DateTimeOffset.Now.ToLocalTime().ToUnixTimeMilliseconds();
                var yesterday = DateTime.Today.ToLocalTime().AddDays(-1);
                var unixYesterday = ((DateTimeOffset) yesterday).ToUnixTimeMilliseconds();
                var listEvent = await _eventRepository.GetLast2Days(unixToday, unixYesterday);
                var listEventSub = await _eventSubmissionRepository.GetLast2Days(unixToday, unixYesterday);
                foreach (var rp in listEvent)
                {
                    if (long.Parse(rp.date) < todayStart)
                    {
                        listReportYesterday.Add(rp);
                    }
                    else
                    {
                        listReportToday.Add(rp);
                    }
                }
                
                foreach (var rp in listEventSub)
                {
                    if (long.Parse(rp.date) < todayStart)
                    {
                        listReportYesterday.Add(rp);
                    }
                    else
                    {
                        listReportToday.Add(rp);
                    }
                }

                notification.today = listReportToday.OrderByDescending(rp => long.Parse(rp.date)).ToList();
                notification.yesterday = listReportYesterday.OrderByDescending(rp => long.Parse(rp.date)).ToList();

                return Ok(notification);
            // }
            // catch (Exception)
            // {
            //     return StatusCode(500, "Get notification error!");
            // }
        }

        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("chart")]
        public async Task<IActionResult> GetChartData()
        {
            var chartData = new Chart();
            var datasets = new List<Datasets>();

            var listOfReceived = new List<int>();
            var listOfDonated = new List<int>();

            var listApprovedTransactions = await _donorTransactionRepository.GetTransactionByStatus(1);
            var listId = listApprovedTransactions.Select(transaction => new Id(transaction.donorId,
                transaction.updateStatusAt, "transaction", transaction._id, transaction.amount)).ToList();

            var listApprovedRequest = await _requestRepository.GetRequestByStatus(1);
            listId.AddRange(listApprovedRequest.Select(request =>
                new Id(request.HospitalId, request.updateStatusAt, "request", request._id, request.Quantity)));

            listId = new List<Id>(listId.OrderByDescending(id => long.Parse(id.date)));
            var firstMonth = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(listId.Last().date)).ToLocalTime();


            var endDate = DateTimeOffset.Now.ToLocalTime();
            var rangeOfMonth = ((endDate.Year - firstMonth.Year) * 12) + endDate.Month - firstMonth.Month;
            var startDate = rangeOfMonth > 5 ? endDate.AddMonths(-5) : endDate.AddMonths(-rangeOfMonth);
            startDate = new DateTime(startDate.Year, startDate.Month, 1);


            var tempDate = startDate;
            chartData.labels = new List<string>();
            while (tempDate <= endDate)
            {
                var month = tempDate.ToString("MMMM");
                chartData.labels.Add(month);

                var received = 0;
                var donated = 0;
                foreach (var data in listId)
                {
                    var date = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(data.date)).ToLocalTime();
                    if (date < startDate || date > endDate) continue;
                    if (date.Month == tempDate.Month)
                    {
                        switch (data.type)
                        {
                            case "transaction":
                                received += data.amount;
                                break;
                            case "request":
                                donated += data.amount;
                                break;
                        }
                    }
                }

                listOfReceived.Add(received);
                listOfDonated.Add(donated);

                tempDate = tempDate.AddMonths(1);
            }

            datasets.Add(new Datasets("Received", listOfReceived));
            datasets.Add(new Datasets("Donated", listOfDonated));

            chartData.datasets = datasets;

            return new JsonResult(chartData);
        }
    }
}