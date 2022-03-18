using System.Collections.Generic;
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
            return donor._id;
        }

        public Task<Donor> Get(string _id)
        {
            var filter = Builders<Donor>.Filter.Eq(d => d._id, _id);
            var donor = _donor.Find(filter).FirstOrDefaultAsync();

            return donor;
        }

        public async Task<IEnumerable<Donor>> Get()
        {
            var donor = await _donor.Find(_ => true).ToListAsync();

            return donor;
        }
        
        public async Task<IEnumerable<DonorTransaction>> GetTransaction(string transactionId)
        {
            var filter = Builders<DonorTransaction>.Filter.Eq(dt => dt.donor_id, transactionId);
            var transaction = await _donorTransaction.Find(filter).ToListAsync();

            return transaction;
        }

        public async Task<bool> Update(string _id, Donor donor)
        {
            var filter = Builders<Donor>.Filter.Eq(d => d._id, _id);
            var update = Builders<Donor>.Update
                .Set(d => d.dob, donor.dob)
                .Set(d => d.gender, donor.gender)
                .Set(d => d.address, donor.address)
                .Set(d => d.phone, donor.phone)
                .Set(d => d.email, donor.email)
                .Set(d => d.blood_type, donor.blood_type)
                .Set(d => d.listTransaction, donor.listTransaction);

            var result = await _donor.UpdateOneAsync(filter, update);
            
            return result.ModifiedCount == 1;
        }

        public async Task<bool> Delete(string _id)
        {
            var filter = Builders<Donor>.Filter.Eq(d => d._id, _id);
            var result = await _donor.DeleteOneAsync(filter);

            return result.DeletedCount == 1;
        }
    }
}