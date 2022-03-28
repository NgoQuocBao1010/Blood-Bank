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
                new("Da Khoa Trung Uong", "Can Tho", "0123456789"),
                new("Hoan My", "Can Tho", "9876543210"),
                new("Benh Vien 121", "Can Tho", "0123456780"),
            };

            _hospitalRepository.AddDefaultData(listHospital);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Hospital hospital)
        {
            var result = await _hospitalRepository.Create(hospital);
            return Ok(new {id = result});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var hospital = await _hospitalRepository.Get(id);
                return Ok(hospital);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Hospital ID error!");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var hospital = await _hospitalRepository.Get();
                return Ok(hospital);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Hospital ID error!");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Hospital hospital)
        {
            try
            {
                var result = await _hospitalRepository.Update(id, hospital);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Hospital ID error!");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _hospitalRepository.Delete(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Hospital ID error!");
            }
        }
    }
}