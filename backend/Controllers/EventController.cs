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

        public EventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
            AddDefaultData();
        }

        public void AddDefaultData()
        {
            var e = _eventRepository.Get();
            if (e.Result.Any()) return;

            var listEvent = new List<Event>
            {
                new("Event 01", new Location("Can Tho", "F+"), "1648252800",
                    2, "This is a blood donation event at F+", 10),
                new("Event 02", new Location("Hau Giang", "Nga 6"), "1648252800",
                    2, "This is a blood donation event at Nga 6", 10),
                new("Event 03", new Location("Kien Giang", "Giong Rieng"), "1648252800",
                    2, "This is a blood donation event at Giong Rieng", 10),
                new("Event 04", new Location("An Giang", "Nha Cua May"), "1648252800",
                    2, "This is a blood donation event at Nha Cua May", 10),
                new("Event 05", new Location("Da Lat", "Nomad Homestay"), "1648252800",
                    2, "This is a blood donation event at Nomad", 10),
                new("Event 06", new Location("Can Tho", "Cafe Station"), "1648252800",
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