using ThreatLens.Api.Models;
using ThreatLens.Api.Services;

namespace ThreatLens.Api.DTOs
{
    public static class ScanMappings
    {

        public static T MapBase<T>(this Scan scan) where T : ScanResponseDto, new() => new()
        {
            Id = scan.Id,
            Target = scan.Target,
            TargetType = scan.TargetType.ToString(),
            CreatedAt = scan.CreatedAtUtc,
            Reputation = scan.Reputation,
            MaliciousDetections = scan.MaliciousDetections,
            SuspiciousDetections = scan.SuspiciousDetections,
            HarmlessDetections = scan.HarmlessDetections,
            UndetectedDetections = scan.UndetectedDetections
        };

        public static ScanResponseDto ToDto(this Scan scan) => scan.MapBase<ScanResponseDto>();

        public static ScanDetailDto ToDetailDto(this Scan scan)
        {
            var dto = scan.MapBase<ScanDetailDto>();
            FillDetails(dto, scan);
            return dto;
        }

        public static AdminScanDto ToAdminDto(this AdminScanItem item)
        {
            var dto = item.Scan.MapBase<AdminScanDto>();
            dto.UserId = item.Scan.UserId;
            dto.UserEmail = item.UserEmail;
            return dto;
        }

        public static AdminScanDetailDto ToAdminDetailDto(this AdminScanItem item)
        {
            var dto = item.Scan.MapBase<AdminScanDetailDto>();
            FillDetails(dto, item.Scan);
            dto.UserId = item.Scan.UserId;
            dto.UserEmail = item.UserEmail;
            return dto;
        }

        // the domain/ip part, written once
        private static void FillDetails(ScanDetailDto dto, Scan scan)
        {
            if (scan.DomainScan != null)
                dto.Domain = new DomainScanDto
                {
                    Registrar = scan.DomainScan.Registrar,
                    Tld = scan.DomainScan.Tld,
                    CreationDateUtc = scan.DomainScan.CreationDateUtc
                };

            if (scan.IpScan != null)
                dto.Ip = new IpScanDto
                {
                    Country = scan.IpScan.Country,
                    AsOwner = scan.IpScan.AsOwner,
                    Asn = scan.IpScan.Asn,
                    Network = scan.IpScan.Network
                };
        }
    }
}
