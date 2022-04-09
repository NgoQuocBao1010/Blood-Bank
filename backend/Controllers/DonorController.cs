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
    [Authorize]
    public class DonorController : ControllerBase
    {
        private readonly IDonorRepository _donorRepository;
        private readonly IDonorTransactionRepository _donorTransactionRepository;
        private readonly IEventRepository _eventRepository;

        public DonorController(IDonorRepository donorRepository, IDonorTransactionRepository donorTransactionRepository,
            IEventRepository eventRepository)
        {
            _donorRepository = donorRepository;
            _donorTransactionRepository = donorTransactionRepository;
            _eventRepository = eventRepository;
        }



        [HttpPost]
        public async Task<IActionResult> Create(ListParticipants data)
        {
            var result = "";
            var eventDonated = await _eventRepository.Get(data.eventId);

            if (_donorTransactionRepository.CheckValidListParticipant(data, eventDonated).Result)
            {
                foreach (var donor in data.listParticipants)
                {
                    // check this participant exists or not
                    var exist = await _donorRepository.Get(donor._id);
                    donor.blood = donor.transaction.blood;
                    // if this participant does not exist
                    if (exist == null)
                    {
                        result = await _donorRepository.Create(donor);
                        if (result == null)
                        {
                            return BadRequest();
                        }
                    }
                    else
                    {
                        var update = await Update(exist._id, donor);
                        if (update == null)
                        {
                            return BadRequest("Cannot update new information for this donor");
                        }

                        result = "Update information for this donor successfully";
                    }

                    // set event, rejectReason, donorId property in transaction
                    donor.transaction.eventDonated = new EventDonated
                    {
                        _id = data.eventId,
                        name = eventDonated.name
                    };
                    donor.transaction.rejectReason = "";
                    donor.transaction.donorId = donor._id;

                    // Create a transaction of a participant
                    var transaction = await _donorTransactionRepository.Create(donor.transaction);

                    if (transaction == null)
                    {
                        return BadRequest();
                    }
                } 
            } 
            else
            {
                return BadRequest("Some participants may have already attended this event");
            }


            return new JsonResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInfo(string id)
        {
            var donor = await _donorRepository.Get(id);
            if (donor == null)
            {
                return NotFound();
            }

            return new JsonResult(donor);
        }


        public async Task<IActionResult> Get(string id)
        {
            var result = new List<Donor>();
            var donor = await _donorRepository.Get(id);
            if (donor == null)
            {
                return NotFound();
            }

            var listTransaction = await _donorTransactionRepository.GetTransactionByDonorAndStatus(donor._id, 0);
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

            var enumerable = donors.ToList();
            if (!enumerable.Any())
            {
                return new JsonResult(donors);
            }

            foreach (var donor in enumerable)
            {
                var tempDonor = await _donorRepository.Get(donor._id);
                var listTransaction = await _donorTransactionRepository.GetTransactionByDonorAndStatus(donor._id, 0);
                foreach (var transaction in listTransaction)
                {
                    tempDonor.transaction = transaction;
                    result.Add(tempDonor);
                    tempDonor = await _donorRepository.Get(donor._id);
                }
            }

            var sortResult = result.OrderByDescending(d => long.Parse(d.transaction.dateDonated));
            return new JsonResult(sortResult);
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetDonorsInfo()
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

            var donors = await _donorRepository.GetListDonorById(listDonorId);
            if (donors == null)
            {
                return NotFound();
            }

            return new JsonResult(donors);
        }
        
        [HttpGet("success")]
        public async Task<IActionResult> GetDonorsSuccess()
        {
            var result = new List<Donor>();
            var donors = await _donorRepository.Get();
            if (donors == null)
            {
                return NotFound();
            }

            var listDonors = donors.ToList();
            if (!listDonors.Any())
            {
                return new JsonResult(donors);
            }

            foreach (var donor in listDonors)
            {
                var tempDonor = await _donorRepository.Get(donor._id);
                var listTransaction = await _donorTransactionRepository.GetTransactionByDonorAndStatus(donor._id, 1);
                foreach (var transaction in listTransaction)
                {
                    tempDonor.transaction = transaction;
                    result.Add(tempDonor);
                    tempDonor = await _donorRepository.Get(donor._id);
                }
            }

            var sortResult = result.OrderByDescending(d => long.Parse(d.transaction.dateDonated));
            return new JsonResult(sortResult);
        }
        
        [HttpGet("failure")]
        public async Task<IActionResult> GetDonorsFailure()
        {
            var result = new List<Donor>();
            var donors = await _donorRepository.Get();
            if (donors == null)
            {
                return NotFound();
            }

            var listDonors = donors.ToList();
            if (!listDonors.Any())
            {
                return new JsonResult(donors);
            }

            foreach (var donor in listDonors)
            {
                var tempDonor = await _donorRepository.Get(donor._id);
                var listTransaction = await _donorTransactionRepository.GetTransactionByDonorAndStatus(donor._id, -1);
                foreach (var transaction in listTransaction)
                {
                    tempDonor.transaction = transaction;
                    result.Add(tempDonor);
                    tempDonor = await _donorRepository.Get(donor._id);
                }
            }

            var sortResult = result.OrderByDescending(d => long.Parse(d.transaction.dateDonated));
            return new JsonResult(sortResult);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Donor donor)
        {
            var result = await _donorRepository.Update(id, donor);
            if (!result)
            {
                return BadRequest();
            }
            return new JsonResult("Update Information Of Donor Successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _donorRepository.Delete(id);
            if (!result)
            {
                return BadRequest();
            }
            return new JsonResult("Delete Information Of Donor Successfully");
        }
    }
}