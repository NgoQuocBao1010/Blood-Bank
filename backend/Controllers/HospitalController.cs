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
    [Authorize(Roles = "admin, hospital")]
    public class HospitalController : ControllerBase
    {
        private readonly IHospitalRepository _hospitalRepository;

        public HospitalController(IHospitalRepository hospitalRepository)
        {
            _hospitalRepository = hospitalRepository;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(Hospital hospital)
        {
            var result = await _hospitalRepository.Create(hospital);
            return Ok(new {id = result});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");
            try
            {
                var hospital = await _hospitalRepository.Get(id);
                if (hospital == null)
                {
                    throw new Exception();
                }

                return Ok(hospital);
            }
            catch (Exception)
            {
                return BadRequest("Hospital ID error!");
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var hospital = await _hospitalRepository.Get();
                if (hospital == null)
                {
                    throw new Exception();
                }

                return Ok(hospital);
            }
            catch (Exception)
            {
                return BadRequest("Hospital ID error!");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Hospital hospital)
        {
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");
            try
            {
                var exist = await _hospitalRepository.Get(id);
                if (exist == null)
                {
                    throw new Exception();
                }

                var result = await _hospitalRepository.Update(id, hospital);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Hospital ID error!");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");
            try
            {
                var exist = await _hospitalRepository.Get(id);
                if (exist == null)
                {
                    throw new Exception();
                }

                var result = await _hospitalRepository.Delete(id);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Hospital ID error!");
            }
        }
    }
}