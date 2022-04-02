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
        

        [HttpPut("approve")]
        public async Task<IActionResult> ApproveRequest(IEnumerable<Request> listRequest)
        {
            try
            {
                foreach (var request in listRequest)
                {
                    var result = await _requestRepository.Get(request._id);
                    result.ApproveStatus = 1;
                    await _requestRepository.Update(request._id, result);
                }

                return Ok("Approve request successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Request ID error!");
            }
        }

        [HttpPut("reject")]
        public async Task<IActionResult> RejectRequest(IEnumerable<Request> listRequest)
        {
            try
            {
                foreach (var request in listRequest)
                {
                    var result = await _requestRepository.Get(request._id);
                    result.ApproveStatus = -1;
                    result.RejectReason = request.RejectReason;
                    await _requestRepository.Update(result._id, result);
                }

                return Ok("Reject request successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Request ID error!");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Request request)
        {
            try
            {
                var hospital = _hospitalRepository.Get(request.HospitalId);
                request.HospitalName = hospital.Result.Name;
                var result = await _requestRepository.Create(request);
                return Ok(new {id = result});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Hospital ID error!");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");
            try
            {
                var result = await _requestRepository.Get(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Request ID error!");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _requestRepository.Get();
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Request ID error!");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Request request)
        {
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");
            try
            {
                var result = await _requestRepository.Update(id, request);
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
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");
            try
            {
                var result = await _requestRepository.Delete(id);
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