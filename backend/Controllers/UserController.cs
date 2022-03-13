using System.Threading.Tasks;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            var exist = await _userRepository.CheckUserEmail(user.email);
            var id = "";
            if (!exist)
            { 
                id = await _userRepository.Create(user);
            }
            else
            {
                id = "User Exists!";
            }
            return new JsonResult(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _userRepository.Get(id);
            return new JsonResult(user);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userRepository.Get();
            return new JsonResult(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, User user)
        {
            var result = await _userRepository.Update(id, user);
            return new JsonResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userRepository.Delete(id);
            return new JsonResult(result);
        }
    }
}