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
        private readonly IHospitalRepository _hospitalRepository;

        public UserController(IUserRepository userRepository, IHospitalRepository hospitalRepository)
        {
            _userRepository = userRepository;
            _hospitalRepository = hospitalRepository;
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
                    "deleted" => Unauthorized("User was deleted!"),
                    _ => Unauthorized("User ID error!")
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

                return Ok(new
                {
                    token,
                    user = existedUser
                });
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
                // Check exist user account.
                var existUser = await _userRepository.CheckUserEmail(user.email);
                if (existUser)
                    return BadRequest("Email existed!");

                // Set role for admin if success.
                user.isAdmin = true;

                // Check create admin account. If not check existed hospital account.
                if (user.hospitalId != null)
                {
                    // Check exist hospital.
                    var existHospital = await _hospitalRepository.Get(user.hospitalId);
                    if (existHospital == null)
                        return BadRequest("Invalid hospital ID!");

                    // Check duplicate hospital user.
                    var existHospitalUser = await _userRepository.CheckHospitalId(user.hospitalId);
                    if (existHospitalUser)
                        return BadRequest("Hospital account existed!");

                    // Set role for hospital if success.
                    user.isAdmin = false;
                }

                // Generate password and create account if success.
                user.password = _userRepository.GeneratePassword(8);
                var newUser = await _userRepository.Create(user);

                return Ok(new
                {
                    newUser.email,
                    newUser.password
                });
            }
            catch (Exception)
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
            catch (Exception)
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
            catch (Exception)
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
            catch (Exception)
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
            catch (Exception)
            {
                return BadRequest("User id error!");
            }
        }

        [HttpGet("readJson")]
        public async Task<IActionResult> ReadJson()
        {
            var defaultData = new DefaultData();
            var json = await DefaultData.ReadJson("default_data.json");
            if (json == null)
            {
                throw new Exception();
            }

            return new JsonResult(json.Events);
        }
    }
}