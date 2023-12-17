using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetServer.Data;
using VetServer.DTO;
using VetServer.Models;
using VetServer.Models.Database;

namespace VetServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CagesController : ControllerBase
    {
        private readonly VetCareDbContext _context;
        private readonly ILogger<DoctorsController> _logger;

        public CagesController(VetCareDbContext context, ILogger<DoctorsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Cages/all-cages
        [HttpGet("all-cages")]
        public IActionResult GetAllCages()
        {
            try
            {
                var allCages = _context.Cages
                    .Select(c => new
                    {
                        c.CageId,
                        CageTemperature = (double)c.CageTemperature,
                        CageOxygen = (double)c.CageOxygen,
                        c.patient_id
                    })
                    .ToList();

                return Ok(allCages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all cages");
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Cages/average-temperature
        [HttpGet("average-temperature")]
        public IActionResult GetAverageTemperature()
        {
            try
            {
                var averageTemperature = _context.Cages
                    .Where(c => c.CageTemperature.HasValue)
                    .Average(c => c.CageTemperature);

                return Ok(averageTemperature);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching average temperature");
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Cages/average-oxygen
        [HttpGet("average-oxygen")]
        public IActionResult GetAverageOxygen()
        {
            try
            {
                var averageOxygen = _context.Cages
                    .Where(c => c.CageOxygen.HasValue)
                    .Average(c => c.CageOxygen);

                return Ok(averageOxygen);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching average oxygen level");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
