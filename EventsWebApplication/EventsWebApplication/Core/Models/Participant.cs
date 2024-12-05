using System.Text.Json.Serialization;

namespace EventsWebApplication.Core.Models
{
    public class Participant
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public string Email { get; set; }
        public int EventId { get; set; }

        [JsonIgnore]
        public Event Event { get; set; }
    }
}
