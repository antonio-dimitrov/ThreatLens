using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ThreatLens.Api.Models.VirusTotal;

namespace ThreatLens.Api.Services
{
    public class VirusTotalService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<VirusTotalService> _logger;

        public VirusTotalService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<VirusTotalService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }
        public Task<VirusTotalIPResponse> GetIpReportAsync(string ipAddress) =>
           GetAsync<VirusTotalIPResponse>($"ip_addresses/{Uri.EscapeDataString(ipAddress)}");


        public Task<VirusTotalDomainResponse> GetDomainReportAsync(string domain) =>
            GetAsync<VirusTotalDomainResponse>($"domains/{Uri.EscapeDataString(domain)}");


        private async Task<T> GetAsync<T>(string path)
        {
            var apiKey = _configuration["VirusTotal:ApiKey"];

            // our own configuration problem: a plain exception (the client gets a generic 500)
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("VirusTotal API key is not configured.");

            using var request = new HttpRequestMessage(HttpMethod.Get, path);
            request.Headers.Add("x-apikey", apiKey);

            HttpResponseMessage response;

            try
            {
                response = await _httpClient.SendAsync(request);
            }
            // CHANGE #2: timeouts and network failures become clean, client-safe errors
            catch (TaskCanceledException ex)
            {
                throw new VirusTotalException(
                    "The scan provider took too long to respond. Please try again.", 504, ex);
            }
            catch (HttpRequestException ex)
            {
                throw new VirusTotalException(
                    "Could not reach the scan provider. Please try again later.", 502, ex);
            }

            using (response)
            {
                if (!response.IsSuccessStatusCode)
                {
                    // CHANGE #3: the raw VirusTotal error goes to the LOG only, never to the client
                    var errorBody = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning(
                        "VirusTotal returned {StatusCode} for {Path}: {Body}",
                        (int)response.StatusCode, path, errorBody);

                    throw response.StatusCode switch
                    {
                        HttpStatusCode.NotFound => new VirusTotalException(
                            "No report is available for this target.", 422),

                        HttpStatusCode.TooManyRequests => new VirusTotalException(
                            "Scan limit reached. Please try again in a minute.", 429),

                        // includes 401/403 (bad API key) and 5xx: nothing the user can fix
                        _ => new VirusTotalException(
                            "The scan provider is currently unavailable.", 502)
                    };
                }

                try
                {
                    var report = await response.Content.ReadFromJsonAsync<T>();

                    return report ?? throw new VirusTotalException(
                        "The scan provider returned an empty report.", 502);
                }
                catch (JsonException ex)
                {
                    throw new VirusTotalException(
                        "The scan provider returned an unreadable report.", 502, ex);
                }
            }

        }
    }
}
