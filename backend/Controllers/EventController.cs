#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IDonorTransactionRepository _donorTransactionRepository;
        private readonly IDonorRepository _donorRepository;
        private readonly IEventSubmissionRepository _eventSubmissionRepository;

        public EventController(IEventRepository eventRepository, IDonorTransactionRepository donorTransactionRepository,
            IDonorRepository donorRepository, IEventSubmissionRepository eventSubmissionRepository)
        {
            _eventRepository = eventRepository;
            _donorRepository = donorRepository;
            _donorTransactionRepository = donorTransactionRepository;
            _eventSubmissionRepository = eventSubmissionRepository;
            AddDefaultData();
        }

        public void AddDefaultData()
        {
            var e = _eventRepository.Get();
            if (e.Result.Any()) return;

            var listEvent = new List<Event>
            {
                new("Health and Wellbeing at work", new Location("Cần Thơ", "F+"), "1612976400000",
                    2, "This is a blood donation event at F+"),
                new("Tell me, do you bleed?", new Location("Hậu Giang", "Nga 6"), "1639069200000",
                    2, "This is a blood donation event at Nga 6"),
                new("We are donors", new Location("Hồ Chí Minh", "Cho Ray"), "1646067600000",
                    2, "This is a blood donation event at Cho Ray"),
                new("Judoh Blood Donations", new Location("An Giang", "Nha Cua May"), "1644080400000",
                    2, "This is a blood donation event at Nha Cua May"),
                new("Judoh Blood Donations - Summer Edition", new Location("Đà Lạt", "Nomad Homestay"), "1644080400000",
                    100, "This is a blood donation event at Nomad"),
                new("Judoh Blood Donations - Chrismas Edition", new Location("Cần Thơ", "Cafe Station"),
                    "1671469200000",
                    2, "This is a blood donation event at Cafe Station")
            };

            _eventRepository.AddDefaultData(listEvent);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Event e)
        {
            var id = await _eventRepository.Create(e);
            if (id == null)
            {
                return BadRequest();
            }
            return new JsonResult(id);
        }
        

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNumberOfParticipants(string id, [FromQuery] string? status)
        {
            var e = await _eventRepository.Get(id);
            if (e == null)
            {
                return BadRequest("Cannot find any Event from this _id");
            }
            if (status is "upcoming")
            {
                var eventSubmission = await _eventSubmissionRepository.GetByEvent(id);
                e.participants = eventSubmission.Count();
            }
            else
            {
                var transactions = await _donorTransactionRepository.GetByEvent(id);
                e.participants = transactions.Count();
            }
            
            return new JsonResult(e);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var e = await _eventRepository.Get();
            if (e == null)
            {
                return NotFound();
            }
            return new JsonResult(e);
        }

        [HttpGet("listParticipants/{id}")]
        public async Task<IActionResult> GetListParticipants(string id)
        {
            var result = new List<Donor>();
            var listDonor = await _donorRepository.Get();
            foreach (var donor in listDonor)
            {
                var transaction = await _donorTransactionRepository.GetByEventAndDonor(donor._id, id);

                if (transaction == null) continue;

                var tempDonor = await _donorRepository.Get(donor._id);
                tempDonor.transaction = transaction;
                result.Add(tempDonor);
            }

            if (!result.Any())
            {
                return NotFound();
            }

            var sortResult = result.OrderByDescending(d => long.Parse(d.transaction.dateDonated));

            return new JsonResult(sortResult);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Event e)
        {
            var result = await _eventRepository.Update(id, e);
            if (!result)
            {
                return BadRequest();
            }
            return new JsonResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _eventRepository.Delete(id);
            if (!result)
            {
                return BadRequest();
            }

            return new JsonResult(result);
        }
    }
}