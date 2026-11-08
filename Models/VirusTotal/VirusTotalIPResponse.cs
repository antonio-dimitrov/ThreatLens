using System.Text.Json.Serialization;

namespace ThreatLens.Api.Models.VirusTotal
{
    public class VirusTotalIPResponse
    {
        [JsonPropertyName("data")]
        public VirusTotalIpData? Data { get; set; }
    }
}
