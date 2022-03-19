using System.Threading.Tasks;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BloodEventController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public BloodEventController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Event bloodEvent)
        {
            var id = await _eventRepository.Create(bloodEvent);
            return new JsonResult(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var bloodEvent = await _eventRepository.Get(id);

            return new JsonResult(bloodEvent);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var bloodEvent = await _eventRepository.Get();
            return new JsonResult(bloodEvent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Event bloodEvent)
        {
            var result = await _eventRepository.Update(id, bloodEvent);
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