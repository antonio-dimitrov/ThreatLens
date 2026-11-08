namespace ThreatLens.Api.DTOs
{
    public class ScanResponseDto
    {
        public int Id { get; set; }
        public string Target { get; set; } = string.Empty;
        public string TargetType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } 

        public int? Reputation { get; set; }
        public int? MaliciousDetections { get; set; }
        public int? SuspiciousDetections { get; set; }
        public int? HarmlessDetections { get; set; }
        public int? UndetectedDetections { get; set; }
    }
}
