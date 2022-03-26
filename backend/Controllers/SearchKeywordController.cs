using System.Threading.Tasks;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/search")]
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


        // [HttpGet("{_id}")]
        // public async Task<IActionResult> Get(string _id)
        // {
        //     var donor = _donorRepository.Get(_id);
        //     if (donor != null)
        //     {
        //         
        //     }  
        // }
    }
}