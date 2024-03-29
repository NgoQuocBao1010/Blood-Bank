﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using Newtonsoft.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;


namespace backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _user;
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventSubmissionRepository _eventSubmissionRepository;
        private readonly IBloodRepository _bloodRepository;
        private readonly IRequestRepository _requestRepository;
        private readonly IDonorRepository _donorRepository;
        private readonly IRecentActivityRepository _recentActivityRepository;
        private readonly IConfiguration _configuration;

        public UserRepository(IMongoClient client, IConfiguration configuration)
        {
            _configuration = configuration;
            _hospitalRepository = new HospitalRepository(client);
            _eventRepository = new EventRepository(client);
            _eventSubmissionRepository = new EventSubmissionRepository(client);
            _bloodRepository = new BloodRepository(client);
            _donorRepository = new DonorRepository(client);
            _requestRepository = new RequestRepository(client);
            _recentActivityRepository = new RecentActivityRepository(client);

            var database = client.GetDatabase("BloodBank");
            var collection = database.GetCollection<User>(nameof(User));

            _user = collection;
            AddDefaultData();
        }

        public async Task<User> Create(User user)
        {
            await _user.InsertOneAsync(user);
            return user;
        }

        public Task<User> Get(string _id)
        {
            var filter = Builders<User>.Filter.Eq(u => u._id, _id);
            var user = _user.Find(filter).FirstOrDefaultAsync();
            return user;
        }

        public Task<User> GetByEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.email, email);
            var user = _user.Find(filter).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> CheckUserEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.email, email);
            var user = await _user.Find(filter).ToListAsync();
            return user.Any();
        }

        public async Task<bool> CheckHospitalId(string hospitalId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.hospitalId, hospitalId);
            var user = await _user.Find(filter).ToListAsync();
            return user.Any();
        }

        public async Task<IEnumerable<User>> Get()
        {
            var user = await _user.Find(_ => true).ToListAsync();
            return user;
        }

        public async Task<long> Update(string _id, User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u._id, _id);
            var update = Builders<User>.Update
                .Set(u => u.email, user.email)
                .Set(u => u.password, user.password)
                .Set(u => u.isAdmin, user.isAdmin)
                .Set(u => u.hospitalId, user.hospitalId);
            var result = await _user.UpdateOneAsync(filter, update);
            return result.ModifiedCount;
        }

        public async Task<bool> Delete(string _id)
        {
            var filter = Builders<User>.Filter.Eq(u => u._id, _id);
            var result = await _user.DeleteOneAsync(filter);
            return result.DeletedCount == 1;
        }

        public string Login(User user)
        {
            // Get user role.
            var role = (user.isAdmin) ? "admin" : "hospital";
            var options = new IdentityOptions();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new("UserID", user._id),
                    new(options.ClaimsIdentity.RoleClaimType, role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials =
                    new SigningCredentials(
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_configuration["ApplicationSettings:JWT_Secret"])),
                        SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return token;
        }

        public bool CheckUserPassword(User user, string password)
        {
            return user.password.Equals(password);
        }

        public string GeneratePassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        
        
        public void AddDefaultData()
        {
            _hospitalRepository.AddDefaultData();
            _eventRepository.AddDefaultData();
            _eventSubmissionRepository.AddDefaultData();
            _bloodRepository.AddDefaultData();
            _donorRepository.AddDefaultData();
            _requestRepository.AddDefaultData();
            _recentActivityRepository.AddDefaultData();

            var user = Get();
            if (user.Result.Any()) return;

            var defaultData = new DefaultData();
            var data = DefaultData.ReadJson();
            
            var hospital = _hospitalRepository.Get();
            for (var i = 0; i < data.Result.Users.Count; i++)
            {
                if (data.Result.Users[i].isAdmin)
                {
                    _user.InsertOne(data.Result.Users[i]);
                    continue;
                }
                data.Result.Users[i].password ??= GeneratePassword(8);
                data.Result.Users[i].hospitalId = hospital.Result.ElementAtOrDefault(i - 1)?._id;
                _user.InsertOne(data.Result.Users[i]);
            }
        }
    }
}