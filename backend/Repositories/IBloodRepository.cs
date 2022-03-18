using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Repositories
{
    public interface IBloodRepository
    {
        // Create
        Task<string> Create(Blood blood);
        
        // Read
        Task<Blood> Get(string _id);
        Task<Blood> GetByType(string blood_type);
        Task<IEnumerable<Blood>> Get();
        
        // Update
        Task<bool> Update(string _id, Blood blood);
        
        // Delete
        Task<bool> Delete(string _id);

    }
}