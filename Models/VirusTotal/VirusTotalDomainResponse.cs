using System.Text.Json.Serialization;

namespace ThreatLens.Api.Models.VirusTotal
{
    public class VirusTotalDomainResponse
    {
        [JsonPropertyName("data")]
        public VirusTotalDomainData? Data { get; set; }
    }
}
