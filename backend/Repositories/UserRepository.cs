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
        private readonly IBloodRepository _bloodRepository;
        private readonly IEventSubmissionRepository _eventSubmissionRepository;
        private readonly IRequestRepository _requestRepository;

        public UserRepository(IMongoClient client, IHospitalRepository hospitalRepository,
            IEventRepository eventRepository, IBloodRepository bloodRepository,
            IEventSubmissionRepository eventSubmissionRepository, IRequestRepository requestRepository)
        {
            var database = client.GetDatabase("BloodBank");
            var collection = database.GetCollection<User>(nameof(User));
            _eventRepository = eventRepository;
            _hospitalRepository = hospitalRepository;
            _bloodRepository = bloodRepository;
            _eventSubmissionRepository = eventSubmissionRepository;
            _requestRepository = requestRepository;

            _user = collection;

            // AddDefaultHospital();
            AddDefaultUser();
            // AddDefaultEvent();
            // AddDefaultBlood();
            // AddDefaultEventSubmission();
            // AddDefaultRequest();
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

        public void AddDefaultEvent()
        {
            var e = _eventRepository.Get();
            if (e.Result.Any()) return;

            var listEvent = new List<Event>
            {
                new("Health and Wellbeing at work", new Location("Cần Thơ", "F+"), "1612976400000",
                    2, "This is a blood donation event at F+"),
                new("Tell me, do you bleed?", new Location("Hậu Giang", "Nga 6"), "1639069200000",
                    2, "This is a blood donation event at Nga 6"),
                new("We are donors", new Location("Hồ Chí Minh", "Cho Ray"), "1646067600000",
                    2, "This is a blood donation event at Cho Ray"),
                new("Judoh Blood Donations", new Location("An Giang", "Nha Cua May"), "1644080400000",
                    2, "This is a blood donation event at Nha Cua May"),
                new("Judoh Blood Donations - Summer Edition", new Location("Đà Lạt", "Nomad Homestay"), "1644080400000",
                    100, "This is a blood donation event at Nomad"),
                new("Judoh Blood Donations - Chrismas Edition", new Location("Cần Thơ", "Cafe Station"),
                    "1671469200000",
                    2, "This is a blood donation event at Cafe Station")
            };

            _eventRepository.AddDefaultData(listEvent);
        }

        public void AddDefaultHospital()
        {
            var hospital = _hospitalRepository.Get();
            if (hospital.Result.Any()) return;
            var listHospital = new List<Hospital>
            {
                new("Hoan My", "Can Tho", "9876543210"),
                new("Da Khoa Trung Uong", "Can Tho", "0123456789"),
                new("Benh Vien 121", "Can Tho", "0123456780"),
            };

            _hospitalRepository.AddDefaultData(listHospital);
        }
        
        public void AddDefaultBlood()
        {
            var blood = _bloodRepository.Get();
            if (blood.Result.Any()) return;
            var listBlood = new List<Blood>
            {
                new Blood("A", "Positive", 0),
                new Blood("A", "Negative", 0),
                new Blood("B", "Positive", 0),
                new Blood("B", "Negative", 0),
                new Blood("O", "Positive", 0),
                new Blood("O", "Negative", 0),
                new Blood("AB", "Positive", 0),
                new Blood("AB", "Negative", 0),
                new Blood("Rh", "Positive", 0),
                new Blood("Rh", "Negative", 0)
            };
            _bloodRepository.AddDefaultData(listBlood);
        }
        
        public void AddDefaultEventSubmission()
        {

            // Get the first eventId from default data.
            var listEvent = _eventRepository.Get();
            var eventId = listEvent.Result.First()._id;

            var eventSubmission = _eventSubmissionRepository.Get();
            if (eventSubmission.Result.Any()) return;

            var listEventSubmission = new List<EventSubmission>
            {
                new(eventId,
                    "093201234567",
                    "Trương Hoàng Thuận",
                    "0123456789",
                    "thuan@gmail.com",
                    "Cần Thơ",
                    "male",
                    "973468800",
                    "1640390400"),
                new(eventId,
                    "093212345678",
                    "Ngô Hồng Quốc Bảo",
                    "1234567890",
                    "bao@gmail.com",
                    "Cần Thơ",
                    "male",
                    "971136000",
                    "1640390400"),
                new(eventId,
                    "093223456789",
                    "Bùi Quốc Trọng",
                    "2345678901",
                    "trong@gmail.com",
                    "Hồ Chí Minh",
                    "male",
                    "958003200",
                    "1640390400"),
                new(eventId,
                    "0932345678912",
                    "Lê Chánh Nhựt",
                    "3456789012",
                    "nhut@gmail.com",
                    "Cần Thơ",
                    "male",
                    "949881600",
                    "1640390400")
            };

            _eventSubmissionRepository.AddDefaultData(listEventSubmission);
        }
        
        public void AddDefaultRequest()
        {
            var requests = _requestRepository.Get();
            if (requests.Result.Any()) return;
            var hospitals = _hospitalRepository.Get();

            var listRequest = new List<Request>();
            if (hospitals != null)
            {
                var listHospital = hospitals.Result.ToList();
                for (var i = 0; i < listHospital.Count; i++)
                {
                    var requestBlood = new RequestBlood("A", "Negative");
                    var request = new Request("1614976400000", 4000, requestBlood, listHospital[i]._id,
                        listHospital[i].Name, 0, "");
                    switch (i)
                    {
                        case 1:
                            requestBlood = new RequestBlood("AB", "Positive");
                            request = new Request("1648659600000", 7000, requestBlood, listHospital[i]._id,
                                listHospital[i].Name, 0, "");
                            break;
                        case 2:
                            requestBlood = new RequestBlood("O", "Negative");
                            request = new Request("1649091600000", 10000, requestBlood, listHospital[i]._id,
                                listHospital[i].Name, 0, "");
                            break;
                    }

                    listRequest.Add(request);
                }
            }

            _requestRepository.AddDefaultData(listRequest);
        }

        public void AddDefaultUser()
        {
            AddDefaultEvent();
            AddDefaultHospital();
            AddDefaultBlood();
            AddDefaultEventSubmission();
            AddDefaultRequest();
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