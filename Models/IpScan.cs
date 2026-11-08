namespace ThreatLens.Api.Models
{
    public class IpScan
    {
        public int ScanId { get; set; }
        public Scan Scan { get; set; } = null!;

        public string? Country { get; set; }
        public string? AsOwner { get; set; }
        public int? Asn { get; set; }
        public string? Network { get; set; }
    }
}
