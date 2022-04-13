using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Repositories
{
    public interface IUserRepository
    {
        // Create
        Task<User> Create(User user);

        // Read
        Task<User> Get(string _id);
        Task<User> GetByEmail(string email);
        Task<bool> CheckUserEmail(string email);
        Task<bool> CheckHospitalId(string hospitalId);
        Task<IEnumerable<User>> Get();

        // Task<DefaultData> ReadJson(string filePath);

        // Update
        Task<long> Update(string _id, User user);

        // Delete
        Task<bool> Delete(string _id);

        void AddDefaultData();

        // Login functions.
        string Login(User user);
        bool CheckUserPassword(User user, string password);
        
        string GeneratePassword(int length);

    }
}