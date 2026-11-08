using System.Text.Json.Serialization;

namespace ThreatLens.Api.Models.VirusTotal
{
    public class VirusTotalDomainAttributes
    {
        [JsonPropertyName("reputation")]
        public int? Reputation { get; set; }

        [JsonPropertyName("last_analysis_stats")]
        public VirusTotalAnalysisStats? LastAnalysisStats { get; set; }

       
        [JsonPropertyName("registrar")]
        public string? Registrar { get; set; }

        [JsonPropertyName("tld")]
        public string? Tld { get; set; }

       
        [JsonPropertyName("creation_date")]
        public long? CreationDate { get; set; }
    }
}
