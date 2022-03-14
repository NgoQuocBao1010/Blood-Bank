using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Repositories
{
    public interface IDonorTransactionRepository
    {
        // Create
        Task<string> Create(DonorTransaction donorTransaction);
        
        // Read
        Task<DonorTransaction> Get(string _id);
        Task<IEnumerable<DonorTransaction>> GetByDonor(string donorId);
        Task<IEnumerable<DonorTransaction>> Get();
        
        // Update
        Task<bool> Update(string _id, DonorTransaction donorTransaction);
        
        // Delete
        Task<bool> Delete(string _id);
    }
}