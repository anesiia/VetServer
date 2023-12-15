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
    public class StatisticsController : ControllerBase
    {
        private readonly VetCareDbContext _context;
        private readonly ILogger<DoctorsController> _logger;

        public StatisticsController(VetCareDbContext context, ILogger<DoctorsController> logger)
        {
            _context = context;
            _logger = logger;
        }



    }
}
