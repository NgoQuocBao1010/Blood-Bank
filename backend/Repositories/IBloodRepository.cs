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
        Task<Blood> GetByNameAndType(string name, string type);
        Task<IEnumerable<Blood>> Get();
        
        // Update
        Task<bool> Update(string _id, Blood blood);
        Task<bool> UpdateQuantity(string name, string type, int amount);
        
        // Delete
        Task<bool> Delete(string _id);

    }
}