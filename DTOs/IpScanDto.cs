namespace ThreatLens.Api.DTOs
{
    public class IpScanDto
    {
        public string? Country { get; set; }
        public string? AsOwner { get; set; }
        public int? Asn { get; set; }
        public string? Network { get; set; }
    }
}
