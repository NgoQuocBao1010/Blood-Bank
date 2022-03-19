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