namespace ThreatLens.Api.Models
{
    public class Scan
    {
        public int Id { get; set; }
        public string Target { get; set; } = string.Empty;

        public TargetType TargetType { get; set; }

        public string UserId { get; set; } = string.Empty; //owner of the scan

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public int? Reputation { get; set; }

        public int? MaliciousDetections { get; set; }

        public int? SuspiciousDetections { get; set; }

        public int? HarmlessDetections { get; set; }

        public int? UndetectedDetections { get; set; }

        public DomainScan? DomainScan { get; set; }
        public IpScan? IpScan { get; set; }
    }
}
