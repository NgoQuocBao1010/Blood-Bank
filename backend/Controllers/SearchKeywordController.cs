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
    // [Authorize]
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
            IDictionary<string, object> dict = result;

            // validate _id
            if (ObjectId.TryParse(_id, out _))
            {
                // Check _id  in Event model
                var e = await _eventRepository.Get(_id);
                if (e != null)
                {
                    dict["Event"] = e;
                }
            
                // Check _id  in DonorTransaction model
                var transaction = await _donorTransactionRepository.Get(_id);
                if (transaction != null)
                {
                    dict["DonorTransaction"] = transaction;
                }
            }
            else
            {
                // Check _id  in Donor model
                var donor = await _donorRepository.Get(_id);
                if (donor != null)
                {
                    dict["Donor"] = donor;
                }
            }

            return dict.Count == 0 ? NotFound("Cannot find any object from this _id") : new JsonResult(result);
        }
    }
}