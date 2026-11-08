namespace ThreatLens.Api.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(string email, string password);
        Task<(bool Succeeded, IEnumerable<string> Errors)> RegisterAsync(string email, string password);
    }
}
