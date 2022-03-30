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
    public class EventSubmissionController : ControllerBase
    {
        private readonly IEventSubmissionRepository _eventSubmissionRepository;
        private readonly IEventRepository _eventRepository;

        private readonly EventController _eventController;

        public EventSubmissionController(IEventSubmissionRepository eventSubmissionRepository,
            IEventRepository eventRepository, IDonorTransactionRepository donorTransactionRepository,
            IDonorRepository donorRepository)
        {
            _eventController = new EventController(eventRepository, donorTransactionRepository, donorRepository);
            _eventSubmissionRepository = eventSubmissionRepository;
            _eventRepository = eventRepository;
            AddDefaultData();
        }

        public void AddDefaultData()
        {
            // Get the first eventId from default data.
            _eventController.AddDefaultData();
            var listEvent = _eventRepository.Get();
            var eventId = listEvent.Result.First()._id;

            var eventSubmission = _eventSubmissionRepository.Get();
            if (eventSubmission.Result.Any()) return;

            var listEventSubmission = new List<EventSubmission>
            {
                new(eventId,
                    "093201234567",
                    "Trương Hoàng Thuận",
                    "0123456789",
                    "thuan@gmail.com",
                    "Cần Thơ",
                    "male",
                    "973468800",
                    "1640390400"),
                new(eventId,
                    "093212345678",
                    "Ngô Hồng Quốc Bảo",
                    "1234567890",
                    "bao@gmail.com",
                    "Cần Thơ",
                    "male",
                    "971136000",
                    "1640390400"),
                new(eventId,
                    "093223456789",
                    "Bùi Quốc Trọng",
                    "2345678901",
                    "trong@gmail.com",
                    "Hồ Chí Minh",
                    "male",
                    "958003200",
                    "1640390400"),
                new(eventId,
                    "0932345678912",
                    "Lê Chánh Nhựt",
                    "3456789012",
                    "nhut@gmail.com",
                    "Cần Thơ",
                    "male",
                    "949881600",
                    "1640390400")
            };

            _eventSubmissionRepository.AddDefaultData(listEventSubmission);
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