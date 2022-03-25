using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            AddDefaultData();
        }

        public void AddDefaultData()
        {
            var user = _userRepository.Get();
            if (user.Result.Any()) return;
            var listUser = new List<User>
            {
                new("admin@gmail.com", "admin", true)
            };

            _userRepository.AddDefaultData(listUser);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(User user)
        {
            var existedUser = await _userRepository.GetByEmail(user.email);

            // Wrong email or password.
            if (existedUser == null || !_userRepository.CheckUserPassword(existedUser, user.password))
                return BadRequest("Wrong username or password!");

            // Execute login.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new("UserID", existedUser._id)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials =
                    new SigningCredentials(
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_configuration["ApplicationSettings:JWT_Secret"])),
                        SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return Ok(new {token});
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            var exist = await _userRepository.CheckUserEmail(user.email);
            var id = "";
            if (!exist)
            {
                if (user.isAdmin)
                {
                    user.hospital_id = null;
                }

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
            if (user == null)
            {
                return NotFound();
            }

            return new JsonResult(user);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userRepository.Get();
            if (user == null)
            {
                return NotFound();
            }

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