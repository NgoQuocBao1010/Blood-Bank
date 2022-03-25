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
    public class HospitalController : ControllerBase
    {
        private readonly IHospitalRepository _hospitalRepository;

        public HospitalController(IHospitalRepository hospitalRepository)
        {
            _hospitalRepository = hospitalRepository;
            AddDefaultData();
        }
        
        public void AddDefaultData()
        {
            var hospital = _hospitalRepository.Get();
            if (hospital.Result.Any()) return;
            var listHospital = new List<Hospital>
            {
                new ("Da Khoa Trung Uong", "Can Tho", "0123456789"),
                new ("Hoan My", "Can Tho", "9876543210"),
                new ("Benh Vien 121", "Can Tho", "0123456780"),
            };

            _hospitalRepository.AddDefaultData(listHospital);
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