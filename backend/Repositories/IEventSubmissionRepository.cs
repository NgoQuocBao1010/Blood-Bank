using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Repositories
{
    public interface IEventSubmissionRepository
    {
        // Create
        Task<string> Create(EventSubmission eventSubmission);
        
        // Read
        Task<EventSubmission> Get(string _id);
        Task<IEnumerable<EventSubmission>> Get();
        Task<IEnumerable<EventSubmission>> GetByEvent(string eventId);
        
        // Update
        Task<bool> Update(string _id, EventSubmission eventSubmission);
        
        // Delete
        Task<bool> Delete(string _id);
    }
}