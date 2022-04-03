using System.Threading.Tasks;
using backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IDonorRepository _donorRepository;
        private readonly IRequestRepository _requestRepository;

        public DashboardController(IDonorRepository donorRepository, IRequestRepository requestRepository)
        {
            _donorRepository = donorRepository;
            _requestRepository = requestRepository;
        }

        public async Task<IActionResult> GetRecentActivities()
        {
            
        }
    }
}