using System.Text.Json.Serialization;

namespace EventsWebApplication.Core.Models
{
    public class User
    {
        public int Id {  get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role {  get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiry { get; set; }
        public int? ParticipantId { get; set; }
        [JsonIgnore]
        public Participant Participant { get; set; }

    }
}
