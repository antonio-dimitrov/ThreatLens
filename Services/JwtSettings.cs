using System.ComponentModel.DataAnnotations;

namespace ThreatLens.Api.Services
{
    public class JwtSettings
    {
        public const string SectionName = "Jwt";

        [Required, MinLength(32)] // HmacSha256 needs a key of at least 256 bits
        public string SecretKey { get; set; } = string.Empty;

        [Required]
        public string Issuer { get; set; } = string.Empty;

        [Required]
        public string Audience { get; set; } = string.Empty;

        [Range(1, 1440)]
        public int ExpiryMinutes { get; set; } = 30;
    }
}
