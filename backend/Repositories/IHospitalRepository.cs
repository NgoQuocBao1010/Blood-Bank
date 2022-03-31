using System.Threading.Tasks;
using System.Collections.Generic;
using backend.Models;

namespace backend.Repositories
{
    public interface IHospitalRepository
    {
        // Create
        Task<string> Create(Hospital hospital);
        
        // Read
        Task<Hospital> Get(string _id);
        Task<IEnumerable<Hospital>> Get();
        Task<Hospital> GetFirstHospital();
        
        // Update
        Task<bool> Update(string _id, Hospital hospital);
        
        // Delete
        Task<bool> Delete(string _id);

        void AddDefaultData(IEnumerable<Hospital> listHospital);
    }
}