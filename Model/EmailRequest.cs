using System.Text.Json.Serialization;

namespace EmailSpamDetectionService.Model
{
    public class EmailRequest
    {
        [JsonPropertyName("emailText")]
        public string? emailText { get; set; }
    }
}
