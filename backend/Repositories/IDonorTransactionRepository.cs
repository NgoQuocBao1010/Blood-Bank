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
        Task<DonorTransaction> GetByEventAndDonor(string _id, string eventId);

        Task<IEnumerable<DonorTransaction>> GetByEvent(string eventId);


        Task<IEnumerable<DonorTransaction>> GetTransactionByDonor(string donorId);
        
        // Update
        Task<bool> ApproveParticipants(string _id, string eventId);
        Task<bool> RejectParticipants(string _id, string eventId, string rejectReason);
        
        // Delete
        Task<bool> Delete(string _id);
        
    }
}