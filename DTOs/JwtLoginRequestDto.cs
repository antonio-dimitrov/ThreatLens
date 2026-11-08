namespace ThreatLens.Api.DTOs
{
    public class JwtLoginRequestDto
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
