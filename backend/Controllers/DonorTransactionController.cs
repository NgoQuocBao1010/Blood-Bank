using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DonorTransactionController : ControllerBase
    {
        private readonly IDonorTransactionRepository _donorTransactionRepository;
        private readonly IBloodRepository _bloodRepository;
        private readonly IDonorRepository _donorRepository;

        public DonorTransactionController(IDonorTransactionRepository donorTransactionRepository,
            IBloodRepository bloodRepository, IDonorRepository donorRepository)
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

        [HttpGet("listDonation/{donorId}")]
        public async Task<IActionResult> GetListTransaction(string donorId)
        {
            var donorTransaction = await _donorTransactionRepository.GetTransactionByDonor(donorId);
            if (donorTransaction == null)
            {
                return NotFound();
            }

            return new JsonResult(donorTransaction);
        }

        [HttpPut("approve")]
        public async Task<IActionResult> UpdateToDonor(List<Participant> listParticipants)
        {
            var result = false;
            foreach (var participant in listParticipants)
            {
                // approve transaction
                result = await _donorTransactionRepository.ApproveParticipants(participant._id, participant.eventId);

                // check to approve successfully
                if (!result)
                {
                    return BadRequest();
                }

                // if approving successfully, add amount of blood in this transaction to quantity in Blood model
                var transaction =
                    await _donorTransactionRepository.GetByEventAndDonor(participant._id, participant.eventId);
                
                await _bloodRepository.UpdateQuantity(transaction.blood.name, transaction.blood.type,
                    transaction.amount);
            }

            return new JsonResult(result);
        }

        [HttpPut("reject")]
        public async Task<IActionResult> RejectParticipant(List<Participant> listParticipants)
        {
            var result = false;
            foreach (var participant in listParticipants)
            {
                // reject transaction
                result = await _donorTransactionRepository.RejectParticipants(participant._id, participant.eventId,
                    participant.rejectReason);

                if (!result)
                {
                    return BadRequest();
                }
            }

            return new JsonResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var donorTransaction = await _donorTransactionRepository.Get(id);
            if (donorTransaction == null)
            {
                return NotFound("Cannot find any transaction with this _id");
            }
            var result = await _donorTransactionRepository.Delete(id);

            return new JsonResult(result);
        }
    }
}