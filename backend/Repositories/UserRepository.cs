using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using MongoDB.Driver;

namespace backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _user;
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventSubmissionRepository _eventSubmissionRepository;
        private readonly IBloodRepository _bloodRepository;
        private readonly IRequestRepository _requestRepository;

        public UserRepository(IMongoClient client)
        {
            _hospitalRepository = new HospitalRepository(client);
            _eventRepository = new EventRepository(client);
            _eventSubmissionRepository = new EventSubmissionRepository(client);
            _bloodRepository = new BloodRepository(client);
            _requestRepository = new RequestRepository(client);
            
            var database = client.GetDatabase("BloodBank");
            var collection = database.GetCollection<User>(nameof(User));

            _user = collection;
            AddDefaultData();
        }

        public async Task<string> Create(User user)
        {
            await _user.InsertOneAsync(user);
            return user._id;
        }

        public Task<User> Get(string _id)
        {
            var filter = Builders<User>.Filter.Eq(u => u._id, _id);
            var user = _user.Find(filter).FirstOrDefaultAsync();
            return user;
        }

        public Task<User> GetByEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.email, email);
            var user = _user.Find(filter).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> CheckUserEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.email, email);
            var user = await _user.Find(filter).ToListAsync();
            return user.Any();
        }

        public async Task<IEnumerable<User>> Get()
        {
            var user = await _user.Find(_ => true).ToListAsync();
            return user;
        }

        public async Task<bool> Update(string _id, User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u._id, _id);
            var update = Builders<User>.Update
                .Set(u => u.email, user.email)
                .Set(u => u.password, user.password)
                .Set(u => u.isAdmin, user.isAdmin)
                .Set(u => u.hospital_id, user.hospital_id);
            var result = await _user.UpdateOneAsync(filter, update);
            return result.ModifiedCount == 1;
        }

        public async Task<bool> Delete(string _id)
        {
            var filter = Builders<User>.Filter.Eq(u => u._id, _id);
            var result = await _user.DeleteOneAsync(filter);
            return result.DeletedCount == 1;
        }

        public void AddDefaultData()
        {
            _hospitalRepository.AddDefaultData();
            _eventRepository.AddDefaultData();
            _eventSubmissionRepository.AddDefaultData();
            _bloodRepository.AddDefaultData();
            _requestRepository.AddDefaultData();
            
            var user = Get();
            if (user.Result.Any()) return;

            var admin = new User("admin@gmail.com", "admin", true);

            _user.InsertOne(admin);


            var hospital = _hospitalRepository.GetFirstHospital();
            var newUser = new User("hoanmy@gmail.com", "hoanmy123", false)
            {
                hospital_id = hospital.Result._id
            };
            _user.InsertOne(newUser);
        }

        public bool CheckUserPassword(User user, string password)
        {
            return user.password.Equals(password);
        }
    }
}