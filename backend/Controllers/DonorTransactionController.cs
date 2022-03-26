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
    public class DonorTransactionController : ControllerBase
    {
        private readonly IDonorTransactionRepository _donorTransactionRepository;
        private readonly IBloodRepository _bloodRepository;
        private readonly IDonorRepository _donorRepository;

        public DonorTransactionController(IDonorTransactionRepository donorTransactionRepository, IBloodRepository bloodRepository, IDonorRepository donorRepository)
        {
            _donorTransactionRepository = donorTransactionRepository;
            _bloodRepository = bloodRepository;
            _donorRepository = donorRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(DonorTransaction donorTransaction)
        {
            var id = await _donorTransactionRepository.Create(donorTransaction);
            return new JsonResult(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var donorTransaction = await _donorTransactionRepository.Get(id);
            if (donorTransaction == null)
            {
                return NotFound();
            }
            return new JsonResult(donorTransaction);
        }
        
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var donorTransaction = await _donorTransactionRepository.Get();
            if (donorTransaction == null)
            {
                return NotFound();
            }
            return new JsonResult(donorTransaction);
        }

        [HttpPut("approve")]
        public async Task<IActionResult> Update(List<Participant> listParticipants)
        {
            var result = false;
            foreach (var participant in listParticipants)
            {
                result = await _donorTransactionRepository.ApproveParticipants(participant._id, participant.eventId);
            }
            
            return new JsonResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            bool result;
            var donorTransaction = await _donorTransactionRepository.Get(id);
            if (donorTransaction == null)
            {
                result = false;
            }
            else
            {
                result = await _donorTransactionRepository.Delete(id);
            }

            return new JsonResult(result);
        }
    }
}