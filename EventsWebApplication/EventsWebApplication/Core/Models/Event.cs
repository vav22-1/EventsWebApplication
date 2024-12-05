namespace EventsWebApplication.Core.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public int MaxParticipants { get; set; }
        public ICollection<Participant> Participants { get; set; } = new List<Participant>();
        public string ImagePath { get; set; }
    }
}
