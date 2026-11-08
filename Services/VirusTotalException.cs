namespace ThreatLens.Api.Services
{
    public class VirusTotalException : Exception
    {
        public int StatusCode { get; }

        public VirusTotalException(string message, int statusCode, Exception? inner = null)
            : base(message, inner)
        {
            StatusCode = statusCode;
        }
    }
}

