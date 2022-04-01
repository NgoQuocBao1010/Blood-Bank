using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Repositories
{
    public interface IDonorRepository
    {
        // Create
        Task<string> Create(Donor donor);
        
        // Read
        Task<Donor> Get(string _id);
        Task<IEnumerable<Donor>> Get();
        
        // List Donor Success
        Task<IEnumerable<Donor>> GetDonorsSuccess(List<string> listDonorId);
        
        // Update
        Task<bool> Update(string _id, Donor donor);
        
        // Delete
        Task<bool> Delete(string _id);
        

    }
}