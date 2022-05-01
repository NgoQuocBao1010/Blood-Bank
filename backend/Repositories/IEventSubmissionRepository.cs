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
        Task<List<EventSubmission>> GetByEvent(string eventId);
        Task<IEnumerable<Report>> GetLast2Days(long today, long yesterday);
        
        // Update
        Task<bool> Update(string _id, EventSubmission eventSubmission);
        
        // Delete
        Task<bool> Delete(string _id);

        void AddDefaultData();
    }
}