using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThreatLens.Api.DTOs;
using ThreatLens.Api.Models;
using ThreatLens.Api.Services;

namespace ThreatLens.Api.Controllers
{
    // Regular users: they only ever see THEIR OWN scans
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ScansController : ControllerBase
    {
        private readonly ILogger<ScansController> _logger;
        private readonly ScanService _scanService;

        public ScansController(ScanService scanService, ILogger<ScansController> logger)
        {
            _scanService = scanService;
            _logger = logger;
        }

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<ActionResult<List<ScanResponseDto>>> GetScans()
        {
            var scans = await _scanService.GetAllScansAsync(UserId);
            return Ok(scans.Select(s => s.ToDto()).ToList());      // CHANGE: shared mapping
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ScanDetailDto>> GetScanById(int id)
        {
            var scan = await _scanService.GetScanByIdAsync(id, UserId);

            if (scan == null)
                return NotFound();

            return Ok(scan.ToDetailDto());
        }

        [HttpPost]
        public async Task<ActionResult<ScanDetailDto>> AddScan(CreateScanDto request)
        {
            var scan = await _scanService.CreateScanAsync(request.Target, UserId);

            if (scan == null)
                return BadRequest("Invalid IP address or domain.");

            _logger.LogInformation("User {UserId} created scan {ScanId}", UserId, scan.Id);

            return Ok(scan.ToDetailDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScan(int id)
        {
            var deleted = await _scanService.DeleteScanAsync(id, UserId);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
