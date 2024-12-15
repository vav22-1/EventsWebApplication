using System.Text.Json.Serialization;

namespace EventsWebApplication.Core.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public int MaxParticipants { get; set; }
        public string? ImagePath { get; set; }
        public ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();
        
    }
}
