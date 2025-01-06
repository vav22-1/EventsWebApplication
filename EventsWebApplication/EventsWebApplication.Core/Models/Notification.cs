using System.Text.Json.Serialization;

namespace EventsWebApplication.Core.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int ParticipantId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public Participant Participant { get; set; }
    }

}
