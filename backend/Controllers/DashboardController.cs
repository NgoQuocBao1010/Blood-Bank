using System;
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
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDonorRepository _donorRepository;
        private readonly IDonorTransactionRepository _donorTransactionRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventSubmissionRepository _eventSubmissionRepository;

        public DashboardController(IDonorRepository donorRepository, IRequestRepository requestRepository,
            IDonorTransactionRepository donorTransactionRepository, IEventRepository eventRepository,
            IEventSubmissionRepository eventSubmissionRepository)
        {
            _donorRepository = donorRepository;
            _requestRepository = requestRepository;
            _donorTransactionRepository = donorTransactionRepository;
            _eventRepository = eventRepository;
            _eventSubmissionRepository = eventSubmissionRepository;
        }


        [HttpGet]
        [Route("recentActivities")]
        public async Task<IActionResult> GetRecentActivities()
        {
            var result = new List<RecentActivities>();
            var listApprovedTransactions = await _donorTransactionRepository.GetTransactionByStatus(1);
            var listId = listApprovedTransactions
                .Select(transaction => new Id(transaction.donorId, transaction.updateStatusAt, "transaction",
                    transaction.amount)).ToList();

            var listApprovedRequest = await _requestRepository.GetRequestByStatus(1);
            listId.AddRange(listApprovedRequest.Select(request =>
                new Id(request._id, request.updateStatusAt, "request", request.Quantity)));

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