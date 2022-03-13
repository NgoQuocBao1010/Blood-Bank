using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
            

            var donor = await _donorRepository.Get(donorTransaction.donor_id);

            var blood = await _bloodRepository.GetByType(donor.blood_type);
            blood.quantity += donorTransaction.volume;
            await _bloodRepository.Update(blood._id, blood);
            return new JsonResult(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var donorTransaction = await _donorTransactionRepository.Get(id);
            return new JsonResult(donorTransaction);
        }
        
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var donorTransaction = await _donorTransactionRepository.Get();
            return new JsonResult(donorTransaction);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, DonorTransaction donorTransaction)
        {
            var result = await _donorTransactionRepository.Update(id, donorTransaction);
            return new JsonResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _donorTransactionRepository.Delete(id);
            return new JsonResult(result);
        }
    }
}