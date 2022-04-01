using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Repositories
{
    public interface IRequestRepository
    {
        // Create
        Task<string> Create(Request request);
        
        // Read
        Task<Request> Get(string _id);
        Task<IEnumerable<Request>> Get();
        
        // Update
        Task<bool> Update(string _id, Request request);
        
        // Delete
        Task<bool> Delete(string _id);

        void AddDefaultData();
        
        void ApproveRequest(Request request);
        void RejectRequest(Request request);
    }
}