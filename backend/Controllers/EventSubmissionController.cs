using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

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
            try
            {
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
            try
            {
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