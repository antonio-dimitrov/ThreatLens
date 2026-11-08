namespace ThreatLens.Api.DTOs
{
    public class AdminScanDetailDto : ScanDetailDto
    {
        public string UserId { get; set; } = string.Empty;
        public string? UserEmail { get; set; }

    }
}
