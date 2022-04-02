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
    public class EventSubmissionController : ControllerBase
    {
        private readonly IEventSubmissionRepository _eventSubmissionRepository;

        public EventSubmissionController(IEventSubmissionRepository eventSubmissionRepository)
        {
            _eventSubmissionRepository = eventSubmissionRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(EventSubmission eventSubmission)
        {
            var result = await _eventSubmissionRepository.Create(eventSubmission);
            return Ok(new {id = result});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");
            var result = await _eventSubmissionRepository.Get(id);
            if (result == null)
            {
                return NotFound();
            }

            return new JsonResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _eventSubmissionRepository.Get();
            if (result == null)
            {
                return NotFound();
            }

            return new JsonResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, EventSubmission eventSubmission)
        {
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");
            var exist = await _eventSubmissionRepository.Get(id);
            if (exist == null)
            {
                return NotFound();
            }
            
            var result = await _eventSubmissionRepository.Update(id, eventSubmission);
            return new JsonResult(result);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");
            var exist = await _eventSubmissionRepository.Get(id);
            if (exist == null)
            {
                return NotFound();
            }
            
            var result = await _eventSubmissionRepository.Delete(id);
            return new JsonResult(result);
        }
    }
}