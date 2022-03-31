#nullable enable
using System;
using System.Collections.Generic;
using System.Dynamic;
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
            var listEvents = await _eventRepository.Get();
            if (listEvents == null)
            {
                return NotFound();
            }
            
            return new JsonResult(listEvents);
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