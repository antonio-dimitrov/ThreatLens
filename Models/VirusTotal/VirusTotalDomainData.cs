using System.Text.Json.Serialization;

namespace ThreatLens.Api.Models.VirusTotal
{
    public class VirusTotalDomainData
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("attributes")]
        public VirusTotalDomainAttributes? Attributes { get; set; }
    }
}
