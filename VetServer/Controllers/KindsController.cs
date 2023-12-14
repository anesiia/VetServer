﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetServer.Data;
using VetServer.DTO;
using VetServer.Models;
using VetServer.Models.Database;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VetServer.Controllers
{
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

        // GET: /all-drugs
        [HttpGet("all-kinds")]
        public async Task<IActionResult> GetKinds()
        {
            try
            {
                var kinds = _context.Kinds.Select(k => new KindDto
                {
                    KindId = k.KindId,
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