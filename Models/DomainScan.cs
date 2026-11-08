namespace ThreatLens.Api.Models
{
    public class DomainScan
    {
        public int ScanId { get; set; }
        public Scan Scan { get; set; } = null!;

        public string? Registrar { get; set; }
        public string? Tld { get; set; }
        public DateTime? CreationDateUtc { get; set; }
    }
}
