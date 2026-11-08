using System.Text.Json.Serialization;

namespace ThreatLens.Api.Models.VirusTotal
{
    public class VirusTotalAnalysisStats
    {
        [JsonPropertyName("malicious")]
        public int? Malicious { get; set; }

        [JsonPropertyName("suspicious")]
        public int? Suspicious { get; set; }

        [JsonPropertyName("harmless")]
        public int? Harmless { get; set; }

        [JsonPropertyName("undetected")]
        public int? Undetected { get; set; }
    }
}
