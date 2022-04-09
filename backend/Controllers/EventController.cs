#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm]Event e)
        {
            if (e.image.Length > 0)
            {
                await using var ms = new MemoryStream();
                await e.image.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                e.binaryImage = Convert.ToBase64String(fileBytes);
            }
            var id = await _eventRepository.Create(e);
            if (id == null)
            {
                return BadRequest();
            }
            return new JsonResult(id);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var listEvents = await _eventRepository.Get();
            if (listEvents == null)
            {
                return NotFound();
            }

            return new JsonResult(listEvents);
        }
        

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetNumberOfParticipants(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");
            var e = await _eventRepository.Get(id);
            if (e == null)
            {
                return NotFound("Cannot find any Event from this _id");
            }

            var time = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            if (time < long.Parse(e.startDate))
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


        [HttpGet("listParticipants/{id}")]
        public async Task<IActionResult> GetListParticipants(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");
            var e = await _eventRepository.Get(id);
            if (e == null)
            {
                return NotFound("Cannot find any Event from this _id");
            }
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
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");
            try
            {
                var exist = await _eventRepository.Get(id);
                if (exist == null)
                {
                    throw new Exception();
                }

                var result = await _eventRepository.Update(id, e);
                return Ok(result);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return BadRequest("Event ID error");
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");

            try
            {
                var exist = await _eventRepository.Get(id);
                if (exist == null)
                {
                    throw new Exception();
                }

                var result = await _eventRepository.Delete(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Event ID error");
            }
        }
    }
}