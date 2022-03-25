using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Repositories
{
    public class DonorTransactionRepository : IDonorTransactionRepository
    {
        private readonly IMongoCollection<DonorTransaction> _donorTransaction;
        private readonly IMongoCollection<Donor> _donor;
        private readonly IMongoCollection<Blood> _blood;

        public DonorTransactionRepository(IMongoClient client)
        {
            var database = client.GetDatabase("BloodBank");
            var collection = database.GetCollection<DonorTransaction>(nameof(DonorTransaction));
            var collection1 = database.GetCollection<Donor>(nameof(Donor));
            var collection2 = database.GetCollection<Blood>(nameof(Blood));

            _donorTransaction = collection;
            _donor = collection1;
            _blood = collection2;
        }


        public async Task<string> Create(DonorTransaction donorTransaction)
        {
            await _donorTransaction.InsertOneAsync(donorTransaction);
            return donorTransaction._id;
        }

        public Task<DonorTransaction> Get(string _id)
        {
            var filter = Builders<DonorTransaction>.Filter.Eq(d => d._id, _id);
            var donorTransaction = _donorTransaction.Find(filter).FirstOrDefaultAsync();

            return donorTransaction;
        }

        public async Task<IEnumerable<DonorTransaction>> GetTransactionByDonor(string donorId)
        {
            var filter = Builders<DonorTransaction>.Filter.Eq(dt => dt.donorId, donorId);
            var donorTransaction = await _donorTransaction.Find(filter).ToListAsync();

            return donorTransaction;
        }

        public async Task<IEnumerable<DonorTransaction>> GetPendingTransaction(string donorId)
        {
            var filter = Builders<DonorTransaction>.Filter.Eq(dt => dt.donorId, donorId)
                         & Builders<DonorTransaction>.Filter.Eq(dt => dt.status, 0);
            var transactions = await _donorTransaction.Find(filter).ToListAsync();

            return transactions;
        }

        public async Task<IEnumerable<DonorTransaction>> Get()
        {
            var donorTransaction = await _donorTransaction.Find(_ => true).ToListAsync();

            return donorTransaction;
        }

        public async Task<bool> ApproveParticipants(string _id, string eventId)
        {
            var filter = Builders<DonorTransaction>.Filter.Eq(d => d.donorId, _id)
                & Builders<DonorTransaction>.Filter.Eq(d => d.eventDonated._id, eventId);
            
            var update = Builders<DonorTransaction>.Update.Set(d => d.status, 1);
            var result = await _donorTransaction.UpdateOneAsync(filter, update);

            return result.ModifiedCount == 1;
        }

        public async Task<bool> Delete(string _id)
        {
            var filter = Builders<DonorTransaction>.Filter.Eq(d => d._id, _id);
            var result = await _donorTransaction.DeleteOneAsync(filter);

            return result.DeletedCount == 1;
        }
    }
}