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
        Task<IEnumerable<DonorTransaction>> GetPendingTransaction(string donorId);
        Task<IEnumerable<DonorTransaction>> Get();

        Task<IEnumerable<DonorTransaction>> GetTransactionByDonor(string donorId);
        // Update
        Task<bool> ApproveParticipants(string _id, string eventId);
        
        // Delete
        Task<bool> Delete(string _id);
    }
}