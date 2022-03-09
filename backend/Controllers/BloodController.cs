using System.Threading.Tasks;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BloodController : ControllerBase
    {
        private readonly IBloodRepository _bloodRepository;

        public BloodController(IBloodRepository bloodRepository)
        {
            _bloodRepository = bloodRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Blood blood)
        {
            var exist = await _bloodRepository.GetByType(blood.blood_type);
            var id = "";
            if (!exist)
            { 
                id = await _bloodRepository.Create(blood);
            }
            else
            {
                id = "Blood Type Exists!";
            }
            return new JsonResult(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var blood = await _bloodRepository.Get(id);
            return new JsonResult(blood);
        }
        
        [HttpGet("type/{blood_type}")]
        public async Task<IActionResult> GetByType(string blood_type)
        {
            var blood = await _bloodRepository.GetByType(blood_type);
            return new JsonResult(blood);
        }
        
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var blood = await _bloodRepository.Get();
            return new JsonResult(blood);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Blood blood)
        {
            var result = await _bloodRepository.Update(id, blood);
            return new JsonResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _bloodRepository.Delete(id);
            return new JsonResult(result);
        }
    }
}