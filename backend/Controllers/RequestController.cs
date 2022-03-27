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
    public class RequestController : ControllerBase
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IHospitalRepository _hospitalRepository;

        public RequestController(IRequestRepository requestRepository, IHospitalRepository hospitalRepository)
        {
            _requestRepository = requestRepository;
            _hospitalRepository = hospitalRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Request request)
        {
            var hospital = _hospitalRepository.Get(request.Hospital._id);
            request.Hospital.Name = hospital.Result.hospital_name;
            var result = await _requestRepository.Create(request);
            return Ok(new {id = result});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _requestRepository.Get(id);
            if (result == null)
            {
                return NotFound();
            }

            return new JsonResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _requestRepository.Get();
            if (result == null)
            {
                return NotFound();
            }

            return new JsonResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Request request)
        {
            var exist = await _requestRepository.Get(id);
            if (exist == null)
            {
                return NotFound();
            }

            var result = await _requestRepository.Update(id, request);
            return Ok(new {result});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var exist = await _requestRepository.Get(id);
            if (exist == null)
            {
                return NotFound();
            }
            
            var result = await _requestRepository.Delete(id);
            return Ok(new {result});
        }
    }
}