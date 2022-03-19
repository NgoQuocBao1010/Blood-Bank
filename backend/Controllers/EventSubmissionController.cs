﻿using System.Threading.Tasks;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            return new JsonResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
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
            var result = await _eventSubmissionRepository.Update(id, eventSubmission);
            return new JsonResult(result);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _eventSubmissionRepository.Delete(id);
            return new JsonResult(result);
        }
    }
}