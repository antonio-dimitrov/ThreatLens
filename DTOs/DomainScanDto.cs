namespace ThreatLens.Api.DTOs
{
    public class DomainScanDto
    {
        public string? Registrar { get; set; }
        public string? Tld { get; set; }
        public DateTime? CreationDateUtc { get; set; }
    }
}
