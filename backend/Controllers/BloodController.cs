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
    public class BloodController : ControllerBase
    {
        private readonly IBloodRepository _bloodRepository;

        public BloodController(IBloodRepository bloodRepository)
        {
            _bloodRepository = bloodRepository;
            AddDefaultData();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Blood blood)
        {
            var exist = await _bloodRepository.GetByName(blood.name);
            var id = "";
            if (exist != null && exist.type.Equals(blood.type))
            {
                id = "Blood Type Exists!";
            }
            else
            {
                id = await _bloodRepository.Create(blood);
            }
            return new JsonResult(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var blood = await _bloodRepository.Get(id);
            if (blood == null)
            {
                return NotFound();
            }
            return new JsonResult(blood);
        }
        
        [HttpGet("type/{blood_type}")]
        public async Task<IActionResult> GetByType(string blood_type)
        {
            var blood = await _bloodRepository.GetByName(blood_type);
            if (blood == null)
            {
                return NotFound();
            }
            return new JsonResult(blood);
        }

        public void AddDefaultData()
        {
            var blood = _bloodRepository.Get();
            if (blood.Result.Any()) return;
            var listBlood = new List<Blood>
            {
                new Blood("A", "Positive", 0),
                new Blood("A", "Negative", 0),
                new Blood("B", "Positive", 0),
                new Blood("B", "Negative", 0),
                new Blood("O", "Positive", 0),
                new Blood("O", "Negative", 0),
                new Blood("AB", "Positive", 0),
                new Blood("AB", "Negative", 0),
                new Blood("Rh", "Positive", 0),
                new Blood("Rh", "Negative", 0)
            };
            _bloodRepository.AddDefaultData(listBlood);
        }
        
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var blood = await _bloodRepository.Get();
            if (blood == null)
            {
                return NotFound();
            }
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