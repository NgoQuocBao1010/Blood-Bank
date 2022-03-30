using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Repositories
{
    public class DonorRepository : IDonorRepository
    {
        private readonly IMongoCollection<Donor> _donor;
        private readonly IMongoCollection<DonorTransaction> _donorTransaction;
        public DonorRepository(IMongoClient client)
        {
            var database = client.GetDatabase("BloodBank");
            var collection = database.GetCollection<Donor>(nameof(Donor));
            var collection1 = database.GetCollection<DonorTransaction>(nameof(DonorTransaction));

            _donor = collection;
            _donorTransaction = collection1;
        }


        public async Task<string> Create(Donor donor)
        {
            await _donor.InsertOneAsync(donor);
            return "Create Successfully";
        }

        public Task<Donor> Get(string idNumber)
        {
            var filter = Builders<Donor>.Filter.Eq(d => d._id, idNumber);
            var donor = _donor.Find(filter).FirstOrDefaultAsync();

            return donor;
        }

        public async Task<IEnumerable<Donor>> Get()
        {
            var donor = await _donor.Find(_ => true).ToListAsync();
            return donor;
        }
        
        // public async Task<IEnumerable<DonorTransaction>> GetTransaction(string transactionId)
        // {
        //     var filter = Builders<DonorTransaction>.Filter.Eq(dt => dt.donorId, transactionId);
        //     var transaction = await _donorTransaction.Find(filter).ToListAsync();
        //
        //     return transaction;
        // }

        public async Task<IEnumerable<Donor>> GetDonorsSuccess(List<string> listDonorId)
        {
            var listDonorSuccess = new List<Donor>();
            foreach (var id in listDonorId)
            {
                var donor = await Get(id);
                listDonorSuccess.Add(donor);
            }

            return listDonorSuccess;
        }

        public async Task<bool> Update(string id, Donor donor)
        {
            var filter = Builders<Donor>.Filter.Eq(d => d._id, id);
            var update = Builders<Donor>.Update
                .Set(d => d.dob, donor.dob)
                .Set(d => d.gender, donor.gender)
                .Set(d => d.address, donor.address)
                .Set(d => d.phone, donor.phone)
                .Set(d => d.email, donor.email)
                .Set(d => d.blood, donor.blood);
            
            var result = await _donor.UpdateOneAsync(filter, update);
            
            return result.ModifiedCount == 1;
        }

        public async Task<bool> Delete(string idNumber)
        {
            var filter = Builders<Donor>.Filter.Eq(d => d._id, idNumber);
            var result = await _donor.DeleteOneAsync(filter);

            return result.DeletedCount == 1;
        }
        

    }
}