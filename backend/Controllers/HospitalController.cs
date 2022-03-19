using System.Threading.Tasks;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HospitalController : ControllerBase
    {
        private readonly IHospitalRepository _hospitalRepository;

        public HospitalController(IHospitalRepository hospitalRepository)
        {
            _hospitalRepository = hospitalRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Hospital hospital)
        {
            var id = await _hospitalRepository.Create(hospital);
            return new JsonResult(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var hospital = await _hospitalRepository.Get(id);
            if (hospital == null)
            {
                return NotFound();
            }
            return new JsonResult(hospital);
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var hospital = await _hospitalRepository.Get();
            if (hospital == null)
            {
                return NotFound();
            }
            return new JsonResult(hospital);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Hospital hospital)
        {
            var result = await _hospitalRepository.Update(id, hospital);
            return new JsonResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _hospitalRepository.Delete(id);
            return new JsonResult(result);
        }
    }
}