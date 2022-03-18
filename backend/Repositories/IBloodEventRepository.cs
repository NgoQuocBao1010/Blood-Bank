using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Bson;

namespace backend.Repositories
{
    public interface IBloodEventRepository
    {
        // Create
        Task<string> Create(BloodEvent bloodEvent);
        
        // Read
        Task<BloodEvent> Get(string _id);
        Task<IEnumerable<BloodEvent>> Get();
        
        // Update
        Task<bool> Update(string _id, BloodEvent bloodEvent);
        
        // Delete
        Task<bool> Delete(string _id);
    }
}