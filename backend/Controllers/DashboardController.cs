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
        [Route("recentActivities")]
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
         Private class Notification JSON:
        [
            {
                "date": "string",
                "eventSubmission": [
                    {},
                ],
                "event": [
                    {},
                ]
            },
        ] 
        */
        private class Notification
        {
            public Notification(string date)
            {
                Date = date;
            }

            public string Date { get; set; }
            public List<EventSubmission> EventSubmission { get; set; }
            public List<Event> Event { get; set; }
        }

        /*
        Function to convert string epoch to DateTime yyyy-mm-dd
        and return string epoch for that DateTime.
        */
        private static string Epoch2String(string epoch)
        {
            var epochDate = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(epoch))
                .DateTime;
            DateTimeOffset dateTimeOffset = new DateTime(epochDate.Year, epochDate.Month, epochDate.Day);
            var date = dateTimeOffset.ToUnixTimeMilliseconds().ToString();
            return date;
        }

        [HttpGet]
        [Route("notification")]
        public async Task<IActionResult> GetDashboardNotification()
        {
            try
            {
                // Create list date to check exist date.
                var listDate = new List<string>();
                var listNotification = new List<Notification>();

                // Get all eventSubmission.
                var listEventSubmission = await _eventSubmissionRepository.Get();
                foreach (var eventSubmission in listEventSubmission)
                {
                    // Check is the current time contained in list or not.
                    var date = Epoch2String(eventSubmission.DateSubmitted);
                    if (!listDate.Contains(date))
                    {
                        /*
                        Add date to list if not, and create new Notification object
                        which contains the current date and eventSubmission.
                        Add new notification to the result list.
                        */
                        listDate.Add(date);
                        var notification = new Notification(date)
                        {
                            EventSubmission = new List<EventSubmission> {eventSubmission},
                            Event = new List<Event>()
                        };
                        listNotification.Add(notification);
                    }
                    else
                    {
                        /*
                        If the current date is exist. Find the notification with that date
                        and add the current eventSubmission to the eventSubmission list.
                        */
                        var notification = listNotification.Find(notification => notification.Date.Equals(date));
                        notification?.EventSubmission.Add(eventSubmission);
                    }
                }

                // Get all event.
                var listEvent = await _eventRepository.Get();
                foreach (var createdEvent in listEvent)
                {
                    // Check is the current time contained in list or not.
                    var date = Epoch2String(createdEvent.DateCreated);
                    if (!listDate.Contains(date))
                    {
                        /*
                        Add date to list if not, and create new Notification object
                        which contains the current date and eventSubmission.
                        Add new notification to the result list.
                        */
                        listDate.Add(date);
                        var notification = new Notification(date)
                        {
                            Event = new List<Event> {createdEvent},
                            EventSubmission = new List<EventSubmission>()
                        };
                        listNotification.Add(notification);
                    }
                    else
                    {
                        /*
                        If the current date is exist. Find the notification with that date
                        and add the current eventSubmission to the eventSubmission list.
                        */
                        var notification = listNotification.Find(notification => notification.Date.Equals(date));
                        notification?.Event.Add(createdEvent);
                    }
                }

                // Return Ok status with the sorted result list.º
                return Ok(listNotification.OrderByDescending(notification => notification.Date));
            }
            catch (Exception)
            {
                return BadRequest("Get notification error!");
            }
        }
    }
}