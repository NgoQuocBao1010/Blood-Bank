using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/search")]
    [Authorize]
    public class SearchKeywordController : ControllerBase
    {
        private readonly IDonorRepository _donorRepository;
        private readonly IDonorTransactionRepository _donorTransactionRepository;
        private readonly IEventRepository _eventRepository;

        public SearchKeywordController(IDonorRepository donorRepository,
            IDonorTransactionRepository donorTransactionRepository, IEventRepository eventRepository)
        {
            _donorRepository = donorRepository;
            _eventRepository = eventRepository;
            _donorTransactionRepository = donorTransactionRepository;
        }


        [HttpGet("{_id}")]
        public async Task<IActionResult> Get(string _id)
        {
            dynamic result = new ExpandoObject();
            result.type = null;

            // validate _id
            if (ObjectId.TryParse(_id, out _))
            {
                // Check _id  in Event model
                var e = await _eventRepository.Get(_id);
                if (e != null)
                {
                    result.type = "Event";
                }
            
                // Check _id  in DonorTransaction model
                var transaction = await _donorTransactionRepository.Get(_id);
                if (transaction != null)
                {
                    result.type = "DonorTransaction";
                }
            }
            else
            {
                // Check _id  in Donor model
                var donor = await _donorRepository.Get(_id);
                if (donor != null)
                {
                    result.type = "Donor";
                }
            }
            

            return result.type == null ? NotFound("Cannot find any object from this _id") : new JsonResult(result);
        }
    }
}