using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetServer.Data;
using VetServer.DTO;
using VetServer.Models;


namespace VetServer.Controllers;

// api/Doctors
[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly VetCareDbContext _context;
    private readonly ILogger<DoctorsController> _logger;
    private readonly IPasswordHasher<Doctors> _passwordHasher;

    public DoctorsController(VetCareDbContext context, ILogger<DoctorsController> logger, IPasswordHasher<Doctors> passwordHasher)
    {
        _context = context;
        _logger = logger;
        _passwordHasher = passwordHasher;
    }

    [HttpGet("all-doctors")]
    public async Task<IActionResult> GetDoctors()
    {
        var doctors = _context.Doctors.Select(d => new DoctorDto
        {
            DoctorId = d.DoctorId,
            DoctorName = d.DoctorName,
            DoctorEmail = d.DoctorEmail,
            DoctorPhone = d.DoctorPhone
        }).ToList();
        return Ok(doctors);
    }

    [HttpPost("register")]
    public async Task<ActionResult> DoctorRegister(DoctorRegistration model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _context.Doctors.AnyAsync(d => d.DoctorEmail == model.Email))
            {
                return BadRequest("Doctor with such email already exists");
            }

            var newDoctor = new Doctors
            {
                DoctorName = model.Name,
                DoctorEmail = model.Email,
                DoctorPhone = model.Phone,
                DoctorAddress = model.Address,
            };

            newDoctor.DoctorPassHash = _passwordHasher.HashPassword(newDoctor, model.Password);

            await _context.Doctors.AddAsync(newDoctor);
            await _context.SaveChangesAsync();

            return Ok("Doctor registration was successful");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during doctor register");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> DoctorLogin([FromBody] DoctorLoginModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorEmail == model.Email);

            if (doctor == null)
            {
                return BadRequest(ModelState);
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(doctor, doctor.DoctorPassHash, model.PassHash);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                return BadRequest(ModelState);
            }

            return Ok(doctor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during login");
            return StatusCode(500, ex.Message);
        }
    }

    

}