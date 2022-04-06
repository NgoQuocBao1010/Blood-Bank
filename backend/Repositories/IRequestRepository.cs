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
        Task<IEnumerable<Request>> GetRequestByHospitalId(string hospitalId);
        Task<IEnumerable<Request>> GetPendingRequest();
        Task<IEnumerable<Request>> GetApprovedRequest();
        Task<IEnumerable<Request>> GetRejectedRequest();
        
        // Update
        Task<bool> Update(string _id, Request request);
        
        // Delete
        Task<bool> Delete(string _id);

        void AddDefaultData();
        
        void ApproveRequest(Request request);
        void RejectRequest(Request request);

        Task<IEnumerable<Request>> GetRequestByStatus(int status);
    }
}