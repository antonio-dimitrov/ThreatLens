using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using ThreatLens.Api.Data;
using ThreatLens.Api.Models;
using ThreatLens.Api.Models.VirusTotal;


namespace ThreatLens.Api.Services
{
    // A scan together with the email of the user who owns it (used by admin endpoints)
    public record AdminScanItem(Scan Scan, string? UserEmail);

    public class ScanService
    {
        private readonly ThreatLensContext _context;
        private readonly VirusTotalService _virusTotalService;

        public ScanService(ThreatLensContext context, VirusTotalService virusTotalService)
        {
            _context = context;
            _virusTotalService = virusTotalService;
        }

        public async Task<List<Scan>> GetAllScansAsync(string userId)
        {
            return await _context.Scans
                .AsNoTracking()
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAtUtc)
                .ToListAsync();
        }

        public async Task<Scan?> GetScanByIdAsync(int id, string userId)
        {
            return await _context.Scans
                .AsNoTracking()
                .Include(s => s.DomainScan)
                .Include(s => s.IpScan)
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
        }

        public async Task<Scan?> CreateScanAsync(string? target, string userId)
        {
            var targetType = TargetValidator.GetTargetType(target);

            if (targetType == null)
                return null;

            var normalizedTarget = target!.Trim();

            int? reputation = null;
            VirusTotalAnalysisStats? stats = null;
            DomainScan? domainScan = null;
            IpScan? ipScan = null;

            switch (targetType.Value)
            {
                case TargetType.Domain:
                    {
                        var report = await _virusTotalService
                            .GetDomainReportAsync(normalizedTarget);

                        var attributes = report.Data?.Attributes;

                        reputation = attributes?.Reputation;
                        stats = attributes?.LastAnalysisStats;

                        // CHANGE: the TODO is now filled in
                        domainScan = new DomainScan
                        {
                            Registrar = attributes?.Registrar,
                            Tld = attributes?.Tld,
                            CreationDateUtc = FromUnixSeconds(attributes?.CreationDate)
                        };
                        break;
                    }

                case TargetType.IpAddress:
                    {
                        var report = await _virusTotalService
                            .GetIpReportAsync(normalizedTarget);

                        var attributes = report.Data?.Attributes;

                        reputation = attributes?.Reputation;
                        stats = attributes?.LastAnalysisStats;

                        // CHANGE: the TODO is now filled in
                        ipScan = new IpScan
                        {
                            Country = attributes?.Country,
                            AsOwner = attributes?.AsOwner,
                            Asn = attributes?.Asn,
                            Network = attributes?.Network
                        };
                        break;
                    }
            }

            var scan = new Scan
            {
                Target = normalizedTarget,
                TargetType = targetType.Value,
                CreatedAtUtc = DateTime.UtcNow,
                Reputation = reputation,
                MaliciousDetections = stats?.Malicious,
                SuspiciousDetections = stats?.Suspicious,
                HarmlessDetections = stats?.Harmless,
                UndetectedDetections = stats?.Undetected,
                UserId = userId,
                DomainScan = domainScan,
                IpScan = ipScan
            };

            _context.Scans.Add(scan);
            await _context.SaveChangesAsync();

            return scan;
        }

        public async Task<bool> DeleteScanAsync(int id, string userId)
        {
            var scan = await _context.Scans
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);

            if (scan == null)
                return false;

            _context.Scans.Remove(scan);
            await _context.SaveChangesAsync();

            return true;
        }

        // ---------- ADMIN ----------
        // These have NO userId filter. Only call them from a controller marked
        // [Authorize(Roles = AppRoles.Admin)].

        public async Task<List<AdminScanItem>> GetAllScansForAdminAsync()
        {
            var rows = await (
                from s in _context.Scans.AsNoTracking()
                join u in _context.Users on s.UserId equals u.Id
                orderby s.CreatedAtUtc descending
                select new { Scan = s, u.Email }).ToListAsync();

            return rows.Select(r => new AdminScanItem(r.Scan, r.Email)).ToList();
        }

        public async Task<AdminScanItem?> GetScanByIdForAdminAsync(int id)
        {
            var scan = await _context.Scans
                .AsNoTracking()
                .Include(s => s.DomainScan)
                .Include(s => s.IpScan)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (scan == null)
                return null;

            var email = await _context.Users
                .Where(u => u.Id == scan.UserId)
                .Select(u => u.Email)
                .FirstOrDefaultAsync();

            return new AdminScanItem(scan, email);
        }

        // NEW helper: VirusTotal dates are Unix timestamps in seconds
        private static DateTime? FromUnixSeconds(long? seconds) =>
            seconds.HasValue
                ? DateTimeOffset.FromUnixTimeSeconds(seconds.Value).UtcDateTime
                : null;
    }

}
