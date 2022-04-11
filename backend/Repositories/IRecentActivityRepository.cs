using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Repositories
{
    public interface IRecentActivityRepository
    {
        // Create
        Task<string> Create(RecentActivity recentActivity);

        void AddDefaultData();
        
        Task<IEnumerable<RecentActivity>> Get();
    }
}