using System.Threading.Tasks;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DonorController : ControllerBase
    {
        private readonly IDonorRepository _donorRepository;

        public DonorController(IDonorRepository donorRepository)
        {
            _donorRepository = donorRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Donor donor)
        {
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
            var donor = await _donorRepository.Get();
            return new JsonResult(donor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Donor donor)
        {
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