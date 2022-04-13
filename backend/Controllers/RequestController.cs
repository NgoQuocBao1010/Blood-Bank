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
    public class RequestController : ControllerBase
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IBloodRepository _bloodRepository;
        private readonly IRecentActivityRepository _recentActivityRepository;

        public RequestController(IRequestRepository requestRepository, IHospitalRepository hospitalRepository,
            IBloodRepository bloodRepository, IRecentActivityRepository recentActivityRepository)
        {
            _requestRepository = requestRepository;
            _hospitalRepository = hospitalRepository;
            _bloodRepository = bloodRepository;
            _recentActivityRepository = recentActivityRepository;
        }

        [HttpPut("approve")]
        public async Task<IActionResult> ApproveRequest(IEnumerable<Request> listRequest)
        {
            try
            {
                foreach (var request in listRequest)
                {
                    // Check empty or approved request.
                    var existRequest = await _requestRepository.Get(request._id);
                    if (existRequest == null) throw new Exception();
                    if (existRequest.Status == 1) throw new Exception("approved");

                    // Check valid quantity request.
                    var blood = _bloodRepository.GetByNameAndType(existRequest.Blood.Name, existRequest.Blood.Type);
                    if (existRequest.Quantity > blood.Result.quantity) throw new Exception("quantity");

                    await _bloodRepository.UpdateQuantity(existRequest.Blood.Name, existRequest.Blood.Type,
                        -existRequest.Quantity);
                    _requestRepository.ApproveRequest(request);

                    var req = await _requestRepository.Get(request._id);
                    var activity = new RecentActivity("Hospital", req.HospitalId, req._id,
                        "minus", req.HospitalName, req.updateStatusAt, req.Quantity);
                    await _recentActivityRepository.Create(activity);
                }

                return Ok("Approve request successfully!");
            }
            catch (Exception e)
            {
                return e.Message switch
                {
                    "quantity" => BadRequest("Not enough blood!"),
                    "approved" => BadRequest("Request already approved!"),
                    _ => BadRequest("Request ID error!")
                };
            }
        }

        [HttpPut("reject")]
        public async Task<IActionResult> RejectRequest(IEnumerable<Request> listRequest)
        {
            try
            {
                foreach (var request in listRequest)
                {
                    var existRequest = await _requestRepository.Get(request._id);
                    if (existRequest == null) throw new Exception();
                    _requestRepository.RejectRequest(request);

                    switch (existRequest.Status)
                    {
                        case -1:
                            throw new Exception("rejected");
                        case 1:
                            await _bloodRepository.UpdateQuantity(existRequest.Blood.Name, existRequest.Blood.Type,
                                existRequest.Quantity);
                            
                            var req = await _requestRepository.Get(request._id);
                            
                            var activity = new RecentActivity("Hospital", req.HospitalId, req._id,
                                "plus", req.HospitalName, req.updateStatusAt, req.Quantity);
                            await _recentActivityRepository.Create(activity);
                            break;
                    }

                }

                return Ok("Reject request successfully!");
            }
            catch (Exception e)
            {
                return e.Message switch
                {
                    "rejected" => BadRequest("Request already rejected!"),
                    _ => BadRequest("Request ID error!")
                };
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Request request)
        {
            try
            {
                // Check valid hospital ID.
                var hospital = await _hospitalRepository.Get(request.HospitalId);
                if (hospital == null) throw new Exception();

                // Get hospital name for request.
                request.HospitalName = hospital.Name;
                var result = await _requestRepository.Create(request);

                // Push request to hospital history.
                var newRequest = await _requestRepository.Get(result);
                var requestHistory = hospital.RequestHistory ?? new List<Request>();
                requestHistory.Add(newRequest);
                hospital.RequestHistory = requestHistory;

                await _hospitalRepository.Update(request.HospitalId, hospital);

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

        [HttpGet("pendingRequest")]
        public async Task<IActionResult> GetPendingRequest()
        {
            try
            {
                var result = await _requestRepository.GetPendingRequest();
                if (result == null)
                {
                    throw new Exception();
                }
                
                var sortResult = result.OrderByDescending(r => long.Parse(r.Date));

                return Ok(sortResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Get request error!");
            }
        }

        [HttpGet("approvedRequest")]
        public async Task<IActionResult> GetApprovedRequest()
        {
            try
            {
                var result = await _requestRepository.GetApprovedRequest();
                if (result == null)
                {
                    throw new Exception();
                }
                var sortResult = result.OrderByDescending(r => long.Parse(r.Date));

                return Ok(sortResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Get request error!");
            }
        }

        [HttpGet("rejectedRequest")]
        public async Task<IActionResult> GetRejectedRequest()
        {
            try
            {
                var result = await _requestRepository.GetRejectedRequest();
                if (result == null)
                {
                    throw new Exception();
                }
                var sortResult = result.OrderByDescending(r => long.Parse(r.Date));

                return Ok(sortResult);
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
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");
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
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");
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