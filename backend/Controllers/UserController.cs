using System;
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
    [Authorize(Roles = "admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("verify")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyToken()
        {
            try
            {
                var userId = User.Claims.First(c => c.Type == "UserID").Value;
                var existUser = await _userRepository.Get(userId);
                if (existUser == null) throw new Exception("deleted");

                return Ok(existUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e.Message switch
                {
                    "deleted" => BadRequest("User was deleted!"),
                    _ => BadRequest("User ID error!")
                };
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(User user)
        {
            try
            {
                // Check wrong email or password.
                var existedUser = await _userRepository.GetByEmail(user.email);
                if (existedUser == null)
                    return BadRequest("Wrong email!");
                if (!_userRepository.CheckUserPassword(existedUser, user.password))
                    return BadRequest("Wrong password!");

                // Execute login.
                var token = _userRepository.Login(existedUser);

                return Ok(new {token});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Login error!");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            try
            {
                // Check exist user or hospital account.
                var existUser = await _userRepository.CheckUserEmail(user.email);
                var existHospital = await _userRepository.CheckHospitalId(user.hospital_id);
                if (existUser || existHospital)
                    return BadRequest("User existed!");

                // Check create admin account. If not check existed hospital account.
                if (user.hospital_id != null) user.isAdmin = true;

                // Generate password and create account if success.
                user.password = _userRepository.GeneratePassword(8);
                var newUser = await _userRepository.Create(user);

                return Ok(new
                {
                    newUser.email,
                    newUser.password
                });
            }
            catch (Exception e)
            {
                return BadRequest("Create user error!");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");
            try
            {
                var user = await _userRepository.Get(id);
                if (user == null)
                {
                    throw new Exception();
                }

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest("User id error!");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var user = await _userRepository.Get();
                if (user == null)
                {
                    throw new Exception();
                }

                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest("Get user error!");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, User user)
        {
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");
            try
            {
                var exist = await _userRepository.Get(id);
                if (exist == null)
                {
                    throw new Exception();
                }

                var result = await _userRepository.Update(id, user);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest("User id error!");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return NotFound("Invalid ID");
            try
            {
                var exist = await _userRepository.Get(id);
                if (exist == null)
                {
                    throw new Exception();
                }

                var result = await _userRepository.Delete(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest("User id error!");
            }
        }
    }
}