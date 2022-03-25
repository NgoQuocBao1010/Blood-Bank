using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Repositories
{
    public interface IBloodRepository
    {
        // Create
        Task<string> Create(Blood blood);
        void AddDefaultData(List<Blood> listBlood);
        
        // Read
        Task<Blood> Get(string _id);
        Task<Blood> GetByName(string name);
        Task<IEnumerable<Blood>> Get();
        
        // Update
        Task<bool> Update(string _id, Blood blood);
        Task<bool> UpdateQuantity(string _id, int amount);
        
        // Delete
        Task<bool> Delete(string _id);

    }
}