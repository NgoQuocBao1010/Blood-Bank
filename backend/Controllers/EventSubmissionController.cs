using System;
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
    [Authorize(Roles = "admin")]
    public class EventSubmissionController : ControllerBase
    {
        private readonly IEventSubmissionRepository _eventSubmissionRepository;
        private readonly IEventRepository _eventRepository;

        public EventSubmissionController(IEventSubmissionRepository eventSubmissionRepository,
            IEventRepository eventRepository)
        {
            _eventSubmissionRepository = eventSubmissionRepository;
            _eventRepository = eventRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(EventSubmission eventSubmission)
        {
            try
            {
                var existEventSubmission = await _eventRepository.Get(eventSubmission.EventId);
                if (existEventSubmission == null) throw new Exception();

                var result = await _eventSubmissionRepository.Create(eventSubmission);
                return Ok(new {id = result});
            }
            catch (Exception)
            {
                return BadRequest("Event ID error!");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");
            try
            {
                var result = await _eventSubmissionRepository.Get(id);
                if (result == null)
                {
                    throw new Exception();
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Event Submission ID error!");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _eventSubmissionRepository.Get();
                if (result == null)
                {
                    throw new Exception();
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Event Submission ID error!");
            }
        }
        
        [HttpGet("event/{id}")]
        public async Task<IActionResult> GetByEvent(string id)
        {
            try
            {
                // Check valid event.
                var existEvent = await _eventRepository.Get(id);
                if (existEvent == null)
                    return BadRequest("Event ID error!");
                
                // Check existing submission for event.
                var result = await _eventSubmissionRepository.GetByEvent(id);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Event ID error!");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, EventSubmission eventSubmission)
        {
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");
            try
            {
                var exist = await _eventSubmissionRepository.Get();
                if (exist == null)
                {
                    throw new Exception();
                }

                var result = await _eventSubmissionRepository.Update(id, eventSubmission);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Event Submission ID error!");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");
            try
            {
                var exist = await _eventSubmissionRepository.Get();
                if (exist == null)
                {
                    throw new Exception();
                }

                var result = await _eventSubmissionRepository.Delete(id);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Event Submission ID error!");
            }
        }
    }
}