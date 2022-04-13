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
        private readonly IEventRepository _eventRepository;
        public DonorRepository(IMongoClient client)
        {
            var database = client.GetDatabase("BloodBank");
            var collection = database.GetCollection<Donor>(nameof(Donor));
            var collection1 = database.GetCollection<DonorTransaction>(nameof(DonorTransaction));

            _eventRepository = new EventRepository(client);
            
            _donor = collection;
            _donorTransaction = collection1;
            AddDefaultData();
        }
        
        public void AddDefaultData()
        {
            var donors = Get();
            if (donors.Result.Any()) return;
            var data = DefaultData.ReadJson();
            var listDonors = data.Result.Donors;
            for (var i = 0; i < listDonors.Count; i++)
            {
                var e = _eventRepository.Get();
                var listEvent = e.Result.ToList();
                var listTransaction = listDonors[i].listParticipants;
                
                foreach (var donor in listTransaction)
                {
                    donor.blood = donor.transaction.blood;
                    _donor.InsertOne(donor);

                    donor.transaction.eventDonated = new EventDonated
                    {
                        _id = listEvent[listEvent.Count - i - 1]._id,
                        name = listEvent[listEvent.Count - i - 1].name
                    };
                    donor.transaction.donorId = donor._id;
                    donor.transaction.updateStatusAt = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
                    _donorTransaction.InsertOne(donor.transaction);
                }
            }
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
        

        public async Task<IEnumerable<Donor>> GetListDonorById(List<string> listDonorId)
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