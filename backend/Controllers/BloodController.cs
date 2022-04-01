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
            var exist = await _bloodRepository.GetByNameAndType(blood.name, blood.type);
            if (exist != null)
            {
                return BadRequest("Blood Type Exists!");
            }
            var id = await _bloodRepository.Create(blood);
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
        
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var blood = await _bloodRepository.Get();
                return Ok(blood);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Blood blood)
        {
            var result = await _bloodRepository.Update(id, blood);
            if (!result)
            {
                return BadRequest();
            }
            return new JsonResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _bloodRepository.Delete(id);
            if (!result)
            {
                return BadRequest();
            }
            return new JsonResult(result);
        }
    }
}