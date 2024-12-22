using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VetServer.Data;
using VetServer.DTO;
using VetServer.Models;
using VetServer.Models.Database;

namespace VetServer.Controllers
{
    //api/Owners
    [Route("api/[controller]")]
    [ApiController]
    public class OwnersController : ControllerBase
    {
        private readonly VetCareDbContext _context;
        private readonly IPasswordHasher<Owners> _passwordHasher;
        private readonly ILogger<OwnersController> _logger;

        public OwnersController(VetCareDbContext context, IPasswordHasher<Owners> passwordHasher, ILogger<OwnersController> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        // POST: /api/Owners/register
        [HttpPost("register")]
        public async Task<ActionResult> Register(OwnerRegistration model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _context.Owners.AnyAsync(ow => ow.OwnerEmail == model.Email))
                {
                    return BadRequest("Doctor with such email already exists");
                }

                var newOwner = new Owners
                {
                    OwnerName = model.Name,
                    OwnerEmail = model.Email,
                    OwnerPhone = model.Phone,
                };

                newOwner.OwnerPassHash = _passwordHasher.HashPassword(newOwner, model.Password);

                await _context.Owners.AddAsync(newOwner);
                await _context.SaveChangesAsync();

                return Ok("New pet owner registration was successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during user register");
                return StatusCode(500, ex.Message);
            }
        }

        // POST: /api/Owners/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] OwnerLoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var owner = await _context.Owners.FirstOrDefaultAsync(ow => ow.OwnerEmail == model.Email);

                if (owner == null)
                {
                    return BadRequest(ModelState);
                }

                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(owner, owner.OwnerPassHash, model.Password);

                if (passwordVerificationResult != PasswordVerificationResult.Success)
                {
                    return BadRequest(ModelState);
                }

                return Ok(owner);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login");
                return StatusCode(500, ex.Message);
            }
        }

        // GET: /api/Owners/owner-animals/5
        [HttpGet("owner-animals/{ownerId}")]
        public IActionResult GetOwnerAnimals(int ownerId)
        {
            try
            {
                // Отримуємо всіх тварини для власника з вказаним ідентифікатором
                var ownerAnimals = _context.Patients
                    .Where(a => a.owner_id == ownerId)
                    .ToList();

                return Ok(ownerAnimals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching owner animals");
                return StatusCode(500, ex.Message);
            }
        }

        // GET: /api/Owners/personal-info/5
        [HttpGet("personal-info/{id}")]
        public async Task<IActionResult> GetOwnerInfo(int id)
        {
            try
            {
                var owner = await _context.Owners.FindAsync(id);

                if (owner == null)
                {
                    return NotFound("there is no owner");
                }
                var ownerDto = new OwnerDto
                {
                    OwnerId = owner.owner_id,
                    OwnerName = owner.OwnerName,
                    OwnerEmail = owner.OwnerEmail,
                    OwnerPhone = owner.OwnerPhone
                };

                return Ok(ownerDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during appointment info loading");
                return StatusCode(500, ex.Message);
            }
        }

        // PUT: /api/Owners/update-personal-info/5
        [HttpPut("update-personal-info/{id}")]
        public async Task<IActionResult> UpdateOwnerInfo(int id, EditOwner model)
        {
            try
            {
                var owner = await _context.Owners.FindAsync(id);
                if (owner == null)
                {
                    return NotFound("There is no owner with the provided ID.");
                }

                owner.OwnerName = model.Name;
                owner.OwnerPhone = model.Phone;

                _context.Update(owner);
                await _context.SaveChangesAsync();

                return Ok("Personal pet-owner details updated successfully.");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Owners/owner-patients/{ownerId}
        [HttpGet("owner-patients/{ownerId}")]
        public async Task<IActionResult> GetPatientsByOwner(int ownerId)
        {
            try
            {
                var ownerIdParam = new SqlParameter("@OwnerId", ownerId);

                var ownersPatients = await _context.OwnersPatients
                    .FromSqlRaw("SELECT * FROM dbo.GetPatientsByOwner(@OwnerId)", ownerIdParam)
                    .ToListAsync();

                return Ok(ownersPatients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the patients by owner");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
