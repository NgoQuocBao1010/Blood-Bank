using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Repositories
{
    public interface IUserRepository
    {
        // Create
        Task<string> Create(User user);

        // Read
        Task<User> Get(string _id);
        Task<User> GetByEmail(string email);
        Task<bool> CheckUserEmail(string email);
        Task<IEnumerable<User>> Get();

        // Update
        Task<long> Update(string _id, User user);

        // Delete
        Task<bool> Delete(string _id);

        void AddDefaultData();

        // Login functions.
        string Login(User user);
        bool CheckUserPassword(User user, string password);
    }
}