using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;
        private readonly IDonorTransactionRepository _donorTransactionRepository;
        private readonly IDonorRepository _donorRepository;

        public EventController(IEventRepository eventRepository, IDonorTransactionRepository donorTransactionRepository, IDonorRepository donorRepository)
        {
            _eventRepository = eventRepository;
            _donorRepository = donorRepository;
            _donorTransactionRepository = donorTransactionRepository;
            AddDefaultData();
        }

        public void AddDefaultData()
        {
            var e = _eventRepository.Get();
            if (e.Result.Any()) return;

            var listEvent = new List<Event>
            {
                new("Health and Wellbeing at work", new Location("Cần Thơ", "F+"), "1612976400000",
                    2, "This is a blood donation event at F+", 10),
                new("Tell me, do you bleed?", new Location("Hậu Giang", "Nga 6"), "1639069200000",
                    2, "This is a blood donation event at Nga 6", 10),
                new("We are donors", new Location("Hồ Chí Minh", "Cho Ray"), "1646067600000",
                    2, "This is a blood donation event at Cho Ray", 10),
                new("Judoh Blood Donations", new Location("An Giang", "Nha Cua May"), "1644080400000",
                    2, "This is a blood donation event at Nha Cua May", 10),
                new("Judoh Blood Donations - Summer Edition", new Location("Đà Lạt", "Nomad Homestay"), "1644080400000",
                    100, "This is a blood donation event at Nomad", 10),
                new("Judoh Blood Donations - Chrismas Edition", new Location("Cần Thơ", "Cafe Station"), "1671469200000",
                    2, "This is a blood donation event at Cafe Station", 10)
            };

            _eventRepository.AddDefaultData(listEvent);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Event e)
        {
            var id = await _eventRepository.Create(e);
            return new JsonResult(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var e = await _eventRepository.Get(id);

            return new JsonResult(e);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var e = await _eventRepository.Get();
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

            var sortResult = result.OrderByDescending(d => long.Parse(d.transaction.dateDonated));

            return new JsonResult(sortResult);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Event e)
        {
            var result = await _eventRepository.Update(id, e);
            return new JsonResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _eventRepository.Delete(id);

            return new JsonResult(result);
        }
    }
}