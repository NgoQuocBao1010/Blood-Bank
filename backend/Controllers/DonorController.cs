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
        public async Task<IActionResult> Create(Donor donor)
        {
            donor.listTransaction = new List<DonorTransaction>();
            var id = await _donorRepository.Create(donor);
            return new JsonResult(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var donor = await _donorRepository.Get(id);
            return new JsonResult(donor);
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var donors = await _donorRepository.Get();
            return new JsonResult(donors);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Donor donor)
        {   
            donor.listTransaction = (List<DonorTransaction>) await _donorTransactionRepository.GetByDonor(id);
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