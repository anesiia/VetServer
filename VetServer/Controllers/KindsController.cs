using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetServer.Data;
using VetServer.DTO;
using VetServer.Models;
using VetServer.Models.Database;

namespace VetServer.Controllers
{
    // api/Kinds
    [Route("api/[controller]")]
    [ApiController]
    public class KindsController : ControllerBase
    {
        private readonly VetCareDbContext _context;
        private readonly ILogger<DrugsController> _logger;

        public KindsController(VetCareDbContext context, ILogger<DrugsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Kinds/all-kinds
        [HttpGet("all-kinds")]
        public async Task<IActionResult> GetKinds()
        {
            try
            {
                var kinds = _context.Kinds.Select(k => new KindDto
                {
                    kind_id = k.kind_id,
                    KindName = k.KindName
                }).ToList();
                return Ok(kinds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during all kinds of animals loading");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
