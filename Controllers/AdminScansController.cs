using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThreatLens.Api.DTOs;
using ThreatLens.Api.Models;
using ThreatLens.Api.Services;

namespace ThreatLens.Api.Controllers
{

    [Authorize(Roles = AppRoles.Admin)]
    [Route("api/admin/scans")]
    [ApiController]
    public class AdminScansController : ControllerBase
    {
        private readonly ScanService _scanService;

        public AdminScansController(ScanService scanService)
        {
            _scanService = scanService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AdminScanDto>>> GetAllScans()
        {
            var items = await _scanService.GetAllScansForAdminAsync();
            return Ok(items.Select(i => i.ToAdminDto()).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdminScanDetailDto>> GetScanById(int id)
        {
            var item = await _scanService.GetScanByIdForAdminAsync(id);

            if (item == null)
                return NotFound();

            return Ok(item.ToAdminDetailDto());
        }

    }
}
