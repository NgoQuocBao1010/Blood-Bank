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
    [Authorize(Roles = "admin")]
    public class EventSubmissionController : ControllerBase
    {
        private readonly IEventSubmissionRepository _eventSubmissionRepository;

        public EventSubmissionController(IEventSubmissionRepository eventSubmissionRepository)
        {
            _eventSubmissionRepository = eventSubmissionRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(EventSubmission eventSubmission)
        {
            try
            {
                var existEventSubmission = await _eventSubmissionRepository.Get(eventSubmission.EventId);
                if (existEventSubmission == null) throw new Exception();

                var result = await _eventSubmissionRepository.Create(eventSubmission);
                return Ok(new {id = result});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
            catch (Exception e)
            {
                Console.WriteLine(e);
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Event Submission ID error!");
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
            catch (Exception e)
            {
                Console.WriteLine(e);
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Event Submission ID error!");
            }
        }
    }
}