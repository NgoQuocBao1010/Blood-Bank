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
        Task<bool> CheckUserEmail(string email);
        Task<IEnumerable<User>> Get();
        
        // Update
        Task<bool> Update(string _id, User user);
        
        // Delete
        Task<bool> Delete(string _id);
        
        void AddDefaultData(IEnumerable<User> listUser);
    }
}