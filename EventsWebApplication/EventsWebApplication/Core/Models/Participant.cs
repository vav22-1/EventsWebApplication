using System.Text.Json.Serialization;

namespace EventsWebApplication.Core.Models
{
    public class Participant
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfRegistration { get; set; }
        public string? Email { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        public ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
