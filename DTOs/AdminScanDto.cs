namespace ThreatLens.Api.DTOs
{
    public class AdminScanDto : ScanResponseDto
    {
        public string UserId { get; set; } = string.Empty;
        public string? UserEmail { get; set; }
    }
}
