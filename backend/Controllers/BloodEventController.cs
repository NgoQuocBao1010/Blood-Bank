using System.Threading.Tasks;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BloodEventController : ControllerBase
    {
        private readonly IBloodEventRepository _bloodEventRepository;

        public BloodEventController(IBloodEventRepository bloodEventRepository)
        {
            _bloodEventRepository = bloodEventRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(BloodEvent bloodEvent)
        {
            var id = await _bloodEventRepository.Create(bloodEvent);
            return new JsonResult(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var bloodEvent = await _bloodEventRepository.Get(id);

            return new JsonResult(bloodEvent);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var bloodEvent = await _bloodEventRepository.Get();
            return new JsonResult(bloodEvent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, BloodEvent bloodEvent)
        {
            var result = await _bloodEventRepository.Update(id, bloodEvent);
            return new JsonResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _bloodEventRepository.Delete(id);

            return new JsonResult(result);
        }
    }
}