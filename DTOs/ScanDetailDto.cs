namespace ThreatLens.Api.DTOs
{
    public class ScanDetailDto : ScanResponseDto
    {
        public DomainScanDto? Domain { get; set; }
        public IpScanDto? Ip { get; set; }
    }
}
