using System.ComponentModel.DataAnnotations;

namespace ThreatLens.Api.DTOs
{
    public class CreateScanDto
    {
        [Required]
        public string Target { get; set; } = string.Empty;
    }
}
