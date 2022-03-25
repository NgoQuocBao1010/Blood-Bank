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
    public class DonorController : ControllerBase
    {
        private readonly IDonorRepository _donorRepository;
        private readonly IDonorTransactionRepository _donorTransactionRepository;

        public DonorController(IDonorRepository donorRepository, IDonorTransactionRepository donorTransactionRepository)
        {
            _donorRepository = donorRepository;
            _donorTransactionRepository = donorTransactionRepository;
        }
        

        [HttpPost]
        public async Task<IActionResult> Create(List<Donor> listDonor)
        {
            var result = "";
            foreach (var donor in listDonor)
            {
                var exist = await _donorRepository.Get(donor._id);
                if (exist == null)
                {
                    donor.blood = donor.transaction.blood;
                    result = await _donorRepository.Create(donor);
                    if (result == null)
                    {
                        return NotFound();
                    }
                }
                else
                {
                    var listTransactionAttended = await _donorTransactionRepository.GetTransactionByDonor(donor._id);
                    if (listTransactionAttended.Any(transaction => transaction.eventDonated._id == donor.transaction.eventDonated._id))
                    {
                        result = donor.name + " has attended the " + donor.transaction.eventDonated.name +
                                 " event already!";
                        return new JsonResult(result);
                    }
                    else
                    {
                        result = "Create Transaction for already Existing Participant Successfully";
                    }
                }

                donor.transaction.rejectReason = "";
                donor.transaction.donorId = donor._id;
                await _donorTransactionRepository.Create(donor.transaction);
            }
            
            return new JsonResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = new List<Donor>();
            var donor = await _donorRepository.Get(id);
            if (donor == null)
            {
                return NotFound();
            }
            var listTransaction = await _donorTransactionRepository.GetPendingTransaction(donor._id);
            foreach (var transaction in listTransaction)
            {
                donor.transaction = transaction;
                result.Add(donor);
                donor = await _donorRepository.Get(id);
            }
            
            return new JsonResult(result);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetParticipants()
        {
            var result = new List<Donor>();
            var donors = await _donorRepository.Get();
            if (donors == null)
            {
                return NotFound();
            }
            
            foreach (var donor in donors)
            {
                var tempDonor = await _donorRepository.Get(donor._id);
                var listTransaction = await _donorTransactionRepository.GetPendingTransaction(donor._id);
                foreach (var transaction in listTransaction)
                {
                    tempDonor.transaction = transaction;
                    result.Add(tempDonor);
                    tempDonor = await _donorRepository.Get(donor._id);
                }
                
            }
            
            return new JsonResult(result);
        }
        
        [HttpGet("success")]
        public async Task<IActionResult> GetDonorSuccess()
        {
            var listDonorId = new List<string>();
            var transactions = await _donorTransactionRepository.Get();
            foreach (var transaction in transactions)
            {
                if (transaction.status == 1 && !listDonorId.Contains(transaction.donorId))
                {
                    listDonorId.Add(transaction.donorId);
                }
            }
            var donors = await _donorRepository.GetDonorsSuccess(listDonorId);
            if (donors == null)
            {
                return NotFound();
            }
            return new JsonResult(donors);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Donor donor)
        {   
            // donor.listTransaction = (List<DonorTransaction>) await _donorTransactionRepository.GetByDonor(id);
            var result = await _donorRepository.Update(id, donor);
            return new JsonResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _donorRepository.Delete(id);
            return new JsonResult(result);
        }
    }
}