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

        public UserRepository(IMongoClient client)
        {
            var database = client.GetDatabase("User");
            var collection = database.GetCollection<User>(nameof(User));

            _user = collection;
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
    }
}