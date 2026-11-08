using System.Text.Json.Serialization;

namespace ThreatLens.Api.Models.VirusTotal
{
    public class VirusTotalIpAttributes
    {
        [JsonPropertyName("reputation")]
        public int? Reputation { get; set; }

        [JsonPropertyName("last_analysis_stats")]
        public VirusTotalAnalysisStats? LastAnalysisStats { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("as_owner")]
        public string? AsOwner { get; set; }

        [JsonPropertyName("asn")]
        public int? Asn { get; set; }

        [JsonPropertyName("network")]
        public string? Network { get; set; }
    }
}
