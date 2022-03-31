using System;
using System.Collections.Generic;
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

        [HttpPut("approve")]
        public async Task<IActionResult> ApproveRequest(IEnumerable<Request> listRequest)
        {
            try
            {
                foreach (var request in listRequest)
                {
                    var result = await _requestRepository.Get(request._id);
                    if (result == null)
                    {
                        throw new Exception();
                    }

                    _requestRepository.ApproveRequest(request);
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
                    if (result == null)
                    {
                        throw new Exception();
                    }

                    _requestRepository.RejectRequest(request);
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
            try
            {
                var result = await _requestRepository.Get(id);
                if (result == null)
                {
                    throw new Exception();
                }

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
                if (result == null)
                {
                    throw new Exception();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Get request error!");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Request request)
        {
            try
            {
                var exist = await _requestRepository.Get();
                if (exist == null)
                {
                    throw new Exception();
                }

                var result = await _requestRepository.Update(id, request);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Request ID error!");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var exist = await _requestRepository.Get();
                if (exist == null)
                {
                    throw new Exception();
                }

                var result = await _requestRepository.Delete(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Request ID error!");
            }
        }
    }
}