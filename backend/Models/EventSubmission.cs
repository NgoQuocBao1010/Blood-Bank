using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class EventSubmission
    {
        public EventSubmission(string eventId, string idCardNumber, string fullName, string phone, string email, string address, string gender, string dob, string latestDonationDate)
        {
            EventId = eventId;
            IdCardNumber = idCardNumber;
            FullName = fullName;
            Phone = phone;
            Email = email;
            Address = address;
            Gender = gender;
            Dob = dob;
            LatestDonationDate = latestDonationDate;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string EventId { get; set; }

        public string IdCardNumber { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string LatestDonationDate { get; set; }
    }
}